using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.HotFix.Logic.PVPScene;
using YouYouServer.Model;
using static YouYouServer.Common.ServerConfig;

namespace YouYouServer.HotFix.Managers
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    [Handler(ConstDefine.SceneManager)]
    public class SceneManager :ISceneManager
    {
        private ServerConfig.Server CurrServer;

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
