using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core;
using static YouYouServer.Common.ServerConfig;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public class SceneManager
    {
        private Server CurrServer;

        /// <summary>
        /// PVP场景字典
        /// </summary>
        public Dictionary<int, PVPScene> PVPSceneDic { get; }

        public SceneManager()
        {
            PVPSceneDic = new Dictionary<int, PVPScene>();
        }

        public void Init()
        {
            CurrServer = GameServerManager.CurrServer;
            if (CurrServer != null)
            {
                List<SceneConfig> lstScenedConfigs = CurrServer.Sceneconfigs;
                if (lstScenedConfigs != null)
                {
                    foreach (var sceneConfig in lstScenedConfigs)
                    {
                        PVPScene pvpScene = new PVPScene(sceneConfig);
                        PVPSceneDic[sceneConfig.SceneId] = pvpScene;
                    }
                }
            }
        }
    }
}
