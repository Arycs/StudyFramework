using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// 资源加载组件
    /// </summary>
    public class ResourceComponent : YouYouBaseComponent,IUpdateComponent
    {
        /// <summary>
        /// 本地文件路径
        /// </summary>
        public string LocalFilePath ;

        /// <summary>
        /// 资源管理器
        /// </summary>
        public ResourceManager ResourceManager { get; private set; }

        /// <summary>
        /// 资源加载管理器
        /// </summary>
        public ResourceLoaderManager ResourceLoaderManager { get; private set; }

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);

            ResourceManager = new ResourceManager();
            ResourceLoaderManager = new ResourceLoaderManager();
#if DISABLE_ASSETBUNDLE
            LocalFilePath = Application.dataPath;
#else
            LocalFilePath = Application.persistentDataPath;
#endif
        }

        #region InitStreamingAssetsBundleInfo 初始化只读区资源包信息
        /// <summary>
        /// 初始化只读区资源包信息
        /// </summary>
        public void InitStreamingAssetsBundleInfo()
        {
            ResourceManager.InitStreamingAssetsBundleInfo();
        }
        #endregion

        #region InitAssetInfo 初始化资源信息
        /// <summary>
        /// 初始化资源信息
        /// </summary>
        public void InitAssetInfo()
        {
            ResourceLoaderManager.InitAssetInfo();
        }
        #endregion

        /// <summary>
        /// 获取路径的最后名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetLastPathName(string path)
        {
            if (path.IndexOf('/')== -1)
            {
                return path;
            }

            return path.Substring(path.LastIndexOf('/') + 1);
        }

        public override void Shutdown()
        {
            ResourceManager.Dispose();
            ResourceLoaderManager.Dispose();
            
            GameEntry.RemoveUpdateComponent(this);
        }

        public void OnUpdate()
        {
            ResourceLoaderManager.OnUpdate();
        }

        /// <summary>
        /// 获取场景的资源包路径
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public string GetSceneAssetBundlePath(string sceneName)
        {
            return string.Format("download/scenes/{0}.assetbundle", sceneName.ToLower());
        }
    }
}
