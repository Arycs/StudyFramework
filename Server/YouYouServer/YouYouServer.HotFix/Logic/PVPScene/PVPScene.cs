using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Model.DataTable;
using static YouYouServer.Common.ServerConfig;

namespace YouYouServer.HotFix.Logic.PVPScene
{
    /// <summary>
    /// PVP场景
    /// </summary>
    public class PVPScene
    {
        /// <summary>
        /// 当前场景配置
        /// </summary>
        public SceneConfig CurrSceneConfig { get; }

        /// <summary>
        /// 当前场景表格数据
        /// </summary>
        public DTSys_SceneEntity CurrSysScene { get; }

        /// <summary>
        /// 场景线字典
        /// </summary>
        public Dictionary<int, PVPSceneLine> PVPSceneLineDic;

        public PVPScene(SceneConfig sceneConfig)
        {
            CurrSceneConfig = sceneConfig;
            CurrSysScene = DataTableManager.Sys_SceneList.GetDic(CurrSceneConfig.SceneId);

            LoadAOIAreaData();

            PVPSceneLineDic = new Dictionary<int, PVPSceneLine>();

            //至少有一个场景线
            PVPSceneLineDic[1] = new PVPSceneLine(1, this);

            Console.WriteLine("场景{0}初始化完毕", CurrSysScene.SceneName);
        }

        /// <summary>
        /// 加载AOI数据
        /// </summary>
        private void LoadAOIAreaData()
        {
            if (!File.Exists(CurrSceneConfig.AOIJsonDataPath))
            {
                Console.WriteLine("读取AOI区域数据失败{0}",CurrSceneConfig.AOIJsonDataPath);
                return;
            }
            string json = File.ReadAllText(CurrSceneConfig.AOIJsonDataPath, Encoding.UTF8);
            List<AOIAreaData> lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AOIAreaData>>(json);

            foreach (var item in lst)
            {
                item.Init();
                Console.WriteLine(item.TopLeftPos);
            }
        }
    }
}
