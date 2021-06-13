using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    /// <summary>
    /// 下载组件
    /// </summary>
    public class DownloadComponent : YouYouBaseComponent, IUpdateComponent
    {
        [Header("写入磁盘的缓存大小(字节)")] public int FlushSize = 1024 * 1024;

        [Header("下载器的数量")] public int DownloadRoutineCount = 5;
        
        /// <summary>
        /// 下载管理器
        /// </summary>
        private DownloadManager m_DownloadManager;

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            m_DownloadManager = new DownloadManager();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onUpdate"></param>
        /// <param name="onComplete"></param>
        public void BeginDownloadSingle(string url, BaseAction<string,ulong,float> onUpdate = null, BaseAction<string> onComplete = null)
        {
            m_DownloadManager.BeginDownloadSingle(url, onUpdate, onComplete);
        }


        /// <summary>
        /// 下载多个文件
        /// </summary>
        /// <param name="lstUrl"></param>
        /// <param name="onDownloadMulitUpdate"></param>
        /// <param name="onDownloadMulitComplete"></param>
        public void BeginDownloadMulit(LinkedList<string> lstUrl,
            BaseAction<int, int, ulong,ulong> onDownloadMulitUpdate = null,
            BaseAction onDownloadMulitComplete = null)
        {
            m_DownloadManager.BeginDownloadMulit(lstUrl, onDownloadMulitUpdate, onDownloadMulitComplete);
        }

        public void OnUpdate()
        {
            m_DownloadManager.OnUpdate();
        }

        public override void Shutdown()
        {
            GameEntry.RemoveUpdateComponent(this);
        }
    }
}