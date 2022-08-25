using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 主资源加载器
    /// </summary>
    public class MainAssetLoaderRoutine
    {
        /// <summary>
        /// 当前的资源信息实体
        /// </summary>
        private AssetEntity m_CurrAssetEntity;

        /// <summary>
        /// 当前的资源实体
        /// </summary>
        private ResourceEntity m_CurrResourceEntity;

        /// <summary>
        /// 当前资源的依赖资源实体链表(临时存储)
        /// </summary>
        private LinkedList<ResourceEntity> m_DependsResourceList = new LinkedList<ResourceEntity>();

        /// <summary>
        /// 需要加载的依赖资源数量
        /// </summary>
        private int m_NeedLoadAssetDependCount = 0;

        /// <summary>
        /// 当前已经加载的依赖资源数量
        /// </summary>
        private int m_CurrLoadAssetDependCount = 0;

        /// <summary>
        /// 著资源加载完毕
        /// </summary>
        /// <returns></returns>
        public BaseAction<ResourceEntity> OnComplete;

        /// <summary>
        /// 主资源包
        /// </summary>
        private AssetBundle m_MainAssetBundle;

        /// <summary>
        /// 以来资源包名字哈希
        /// </summary>
        private HashSet<string> m_DependsAssetBundleNames = new HashSet<string>();
        
        /// <summary>
        /// 加载主资源
        /// </summary>
        /// <param name="assetCategory"></param>
        /// <param name="assetFullName"></param>
        /// <param name="onComplete"></param>
        public async UniTask<ResourceEntity> Load(AssetCategory assetCategory, string assetFullName,BaseAction<ResourceEntity> onComplete = null)
        {
#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
            Debug.LogError("取池");
            m_CurrResourceEntity = GameEntry.Pool.DequeueClassObject<ResourceEntity>();
            m_CurrResourceEntity.Category = assetCategory;
            m_CurrResourceEntity.IsAssetBundle = false;
            m_CurrResourceEntity.ResourceName = assetFullName;
            m_CurrResourceEntity.Target = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetFullName);
            return m_CurrResourceEntity;
#else
            OnComplete = onComplete;
            m_CurrAssetEntity = GameEntry.Resource.ResourceLoaderManager.GetAssetEntity(assetCategory, assetFullName);
            if (m_CurrAssetEntity == null)
            {
                GameEntry.LogError("assetFullName no exists " + assetFullName);
                return;
            }
            LoadMainAsset();
#endif
        }

        /// <summary>
        /// 真正的加载主资源
        /// </summary>
        private void LoadMainAsset()
        {
            //1. 从分类资源池(AssetPool)中查找
            m_CurrResourceEntity = GameEntry.Pool.AssetPool[m_CurrAssetEntity.Category]
                .Spawn(m_CurrAssetEntity.AssetFullName);
            if (m_CurrResourceEntity != null)
            {
                //Debug.LogError("从分类资源池加载" + assetEntity.ResourceName);
                //说明资源在分类资源池中存在
                OnComplete?.Invoke(m_CurrResourceEntity);
                return;
            }

            //2. 加载这个资源所依赖的资源包
            List<AssetDependsEntity> dependsAssetList = m_CurrAssetEntity.DependsAssetList;
            if (dependsAssetList != null)
            {
                foreach (var assetDependsEntity in dependsAssetList)
                {
                    var assetEntity =
                        GameEntry.Resource.ResourceLoaderManager.GetAssetEntity(assetDependsEntity.Category,
                            assetDependsEntity.AssetFullName);
                    m_DependsAssetBundleNames.Add(assetEntity.AssetBundleName);
                }
            }
            
            //3. 循环依赖哈希  加入任务组
            TaskGroup taskGroup = GameEntry.Task.CreateTaskGroup();
            foreach (var bundleName in m_DependsAssetBundleNames)
            {
                TaskRoutine taskRoutine = GameEntry.Task.CreateTaskRoutine();
                taskRoutine.CurrTask = () =>
                {
                    //依赖资源 只是加载资源包
                    GameEntry.Resource.ResourceLoaderManager.LoadAssetBundle(bundleName,onComplete:(bundle =>
                    {
                        taskRoutine.Leave();
                    }));
                };
                taskGroup.AddTask(taskRoutine);
            }
            
            //4. 加载主资源包
            TaskRoutine taskRoutineLoadMain = GameEntry.Task.CreateTaskRoutine();
            taskRoutineLoadMain.CurrTask = () =>
            {
                GameEntry.Resource.ResourceLoaderManager.LoadAssetBundle(m_CurrAssetEntity.AssetBundleName,onComplete:(
                    bundle =>
                    {
                        m_MainAssetBundle = bundle;
                        taskRoutineLoadMain.Leave();
                    }));
            };
            taskGroup.AddTask(taskRoutineLoadMain);

            taskGroup.OnComplete = () =>
            {
                if (m_MainAssetBundle ==  null)
                {
                    GameEntry.LogError($"MainAssetBundle not exists {m_CurrAssetEntity.AssetFullName}");
                }
                GameEntry.Resource.ResourceLoaderManager.LoadAsset(m_CurrAssetEntity.AssetFullName,m_MainAssetBundle,onComplete:(
                    obj =>
                    {
                        //再次检查 很重要 不检查引用计数会出错
                        m_CurrResourceEntity = GameEntry.Pool.AssetPool[m_CurrAssetEntity.Category]
                            .Spawn(m_CurrAssetEntity.AssetFullName);
                        if (m_CurrResourceEntity != null)
                        {
                            OnComplete ?.Invoke(m_CurrResourceEntity);
                            Reset();
                            return;
                        }
                        m_CurrResourceEntity = GameEntry.Pool.DequeueClassObject<ResourceEntity>();
                        m_CurrResourceEntity.Category = m_CurrAssetEntity.Category;
                        m_CurrResourceEntity.IsAssetBundle = false;
                        m_CurrResourceEntity.ResourceName = m_CurrAssetEntity.AssetFullName;
                        m_CurrResourceEntity.Target = obj;
                        
                        GameEntry.Pool.AssetPool[m_CurrAssetEntity.Category].Register(m_CurrResourceEntity);
                        OnComplete?.Invoke(m_CurrResourceEntity);
                        Reset();
                    }));
            };
            
            taskGroup.Run(true);
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset()
        {
            OnComplete = null;
            m_CurrAssetEntity = null;
            m_CurrResourceEntity = null;
            m_NeedLoadAssetDependCount = 0;
            m_CurrLoadAssetDependCount = 0;
            m_DependsResourceList.Clear();
            GameEntry.Pool.EnqueueClassObject(this);
        }
    }
}