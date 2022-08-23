using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace YouYou
{
    public class DataTableManager : ManagerBase, IDisposable
    {
        public DataTableManager()
        {
            InitDBModel();
        }

        /// <summary>
        /// 总共需要加载的表格数量
        /// </summary>
        public int TotalTableCount = 0;

        /// <summary>
        /// 当前需要加载的表格数量
        /// </summary>
        public int CurrLoadTableCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public DTSys_CodeDBModel Sys_CodeDBModel { get; private set; }

        /// <summary>
        /// 系统特效表
        /// </summary>
        public DTSys_EffectDBModel Sys_EffectDBModel { get; private set; }

        /// <summary>
        /// 系统预置体表
        /// </summary>
        public DTSys_PrefabDBModel Sys_PrefabDBModel { get; private set; }

        /// <summary>
        /// 系统声音表
        /// </summary>
        public DTSys_SoundDBModel Sys_SoundDBModel { get; private set; }

        /// <summary>
        /// 系统剧情声音表
        /// </summary>
        public DTSys_StorySoundDBModel Sys_StorySoundDBModel { get; private set; }

        /// <summary>
        /// 系统UI预制表
        /// </summary>
        public DTSys_UIFormDBModel Sys_UIFormDBModel { get; private set; }

        /// <summary>
        /// 系統本地化表
        /// </summary>
        public LocalizationDBModel LocalizationDBModel { get; private set; }

        /// <summary>
        /// 系统场景表
        /// </summary>
        public DTSys_SceneDBModel Sys_SceneDBModel { get; private set; }

        /// <summary>
        /// 系统场景详情表
        /// </summary>
        public DTSys_SceneDetailDBModel Sys_SceneDetailDBModel { get; private set; }

        /// <summary>
        /// 音效表
        /// </summary>
        public DTSys_AudioDBModel Sys_AudioDBModel { get; private set; }

        /// <summary>
        /// 职业表
        /// </summary>
        public DTJobDBModel JobList { get; private set; }

        /// <summary>
        /// 角色动画表
        /// </summary>
        public DTRoleAnimationDBModel RoleAnimationList { get; private set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public DTBaseRoleDBModel BaseRoleList { get; private set; }

        public DTSpriteDBModel SpriteList { get; private set; }
        /// <summary>
        /// 角色对应动画列表
        /// </summary>
        public DTRoleAnimCategoryDBModel RoleAnimCategoryList { get; private set; }

        /// <summary>
        /// 初始化DBModel
        /// </summary>
        private void InitDBModel()
        {
            //每个表都需要new一下
            Sys_CodeDBModel = new DTSys_CodeDBModel();
            Sys_EffectDBModel = new DTSys_EffectDBModel();
            LocalizationDBModel = new LocalizationDBModel();
            Sys_PrefabDBModel = new DTSys_PrefabDBModel();
            Sys_SoundDBModel = new DTSys_SoundDBModel();
            Sys_StorySoundDBModel = new DTSys_StorySoundDBModel();
            Sys_UIFormDBModel = new DTSys_UIFormDBModel();
            Sys_SceneDBModel = new DTSys_SceneDBModel();
            Sys_SceneDetailDBModel = new DTSys_SceneDetailDBModel();
            Sys_AudioDBModel = new DTSys_AudioDBModel();

            JobList = new DTJobDBModel();
            RoleAnimationList = new DTRoleAnimationDBModel();
            BaseRoleList = new DTBaseRoleDBModel();
            SpriteList = new DTSpriteDBModel();
            RoleAnimCategoryList = new DTRoleAnimCategoryDBModel();
        }


        /// <summary>
        /// 加载表格
        /// </summary>
        public void LoadDataTable()
        {
            //每个表都需要LoadData
            Sys_CodeDBModel.LoadData();
            Sys_EffectDBModel.LoadData();
            LocalizationDBModel.LoadData();
            Sys_PrefabDBModel.LoadData();
            Sys_SoundDBModel.LoadData();
            Sys_StorySoundDBModel.LoadData();
            Sys_UIFormDBModel.LoadData();
            Sys_SceneDBModel.LoadData();
            Sys_SceneDetailDBModel.LoadData();
            Sys_AudioDBModel.LoadData();
            
            JobList.LoadData();
            RoleAnimationList.LoadData();
            BaseRoleList.LoadData();
            RoleAnimCategoryList.LoadData();
        }

        /// <summary>
        /// 表格资源包
        /// </summary>
        private AssetBundle m_DataTableBundle;

        /// <summary>
        /// 异步加载表格
        /// </summary>
        public void LoadDataTableAsync()
        {
#if DISABLE_ASSETBUNDLE
            LoadDataTable();
#else
            GameEntry.Resource.ResourceLoaderManager.LoadAssetBundle("download/datatable.assetbundle",
                onComplete: (AssetBundle bundle) =>
                {
                    m_DataTableBundle = bundle;
                    Debug.LogError("LoadDataTableAsync 拿到了 bundle");
                    LoadDataTable();
                });
#endif
        }

        /// <summary>
        /// 获取表格的字节数组
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="onComplete"></param>
        public void GetDataTableBuffer(string tableName, BaseAction<byte[]> onComplete)
        {
#if DISABLE_ASSETBUNDLE
            GameEntry.Time.Yield(() =>
            {
                byte[] buffer = IOUtil.GetFileBuffer(string.Format("{0}/download/DataTable/{1}.bytes",
                    GameEntry.Resource.LocalFilePath, tableName));
                if (onComplete != null)
                {
                    onComplete(buffer);
                }
            });
#else
            GameEntry.Resource.ResourceLoaderManager.LoadAsset(GameEntry.Resource.GetLastPathName(tableName),
                m_DataTableBundle, onComplete:
                (UnityEngine.Object obj) =>
                {
                    TextAsset asset = obj as TextAsset;
                    onComplete?.Invoke(asset.bytes);
                });
#endif
        }


        private void Clear()
        {
            //每个表都Clear
            Sys_CodeDBModel.Clear();
            Sys_EffectDBModel.Clear();
            LocalizationDBModel.Clear();
            Sys_PrefabDBModel.Clear();
            Sys_SoundDBModel.Clear();
            Sys_StorySoundDBModel.Clear();
            Sys_UIFormDBModel.Clear();
            Sys_SceneDBModel.Clear();
            Sys_SceneDetailDBModel.Clear();
            Sys_AudioDBModel.Clear();
            
            JobList.Clear();
            RoleAnimationList.Clear();
            BaseRoleList.Clear();
            RoleAnimCategoryList.Clear();
        }

        public void Dispose()
        {
            Clear();
        }

        public override void Init()
        {
        }
    }
}