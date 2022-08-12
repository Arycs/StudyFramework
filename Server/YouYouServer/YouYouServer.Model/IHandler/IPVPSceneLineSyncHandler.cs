using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.Model.IHandler
{
    public interface IPVPSceneLineSyncHandler
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pvpSceneLine"></param>
        public void Init(PVPSceneLine pvpSceneLine);

        /// <summary>
        /// 同步状态
        /// </summary>
        public void SyncStatus();

        public void Dispose();
    }
}
