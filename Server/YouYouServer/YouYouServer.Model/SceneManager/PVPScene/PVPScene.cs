using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YouYouServer.Common;
using YouYouServer.Model.DataTable;
using static YouYouServer.Common.ServerConfig;

namespace YouYouServer.Model.ServerManager
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

        /// <summary>
        /// 当前场景的区域数据列表
        /// </summary>
        public List<AOIAreaData> CurrSceneAreaDataList { get; private set; }

        /// <summary>
        /// AOI区域字典
        /// </summary>
        private Dictionary<int, AOIAreaData> m_AOIAreaDataDic;

        public PVPScene(SceneConfig sceneConfig)
        {
            CurrSceneConfig = sceneConfig;
            CurrSysScene = DataTableManager.Sys_SceneList.GetDic(CurrSceneConfig.SceneId);
            CurrSceneAreaDataList = new List<AOIAreaData>();
            m_AOIAreaDataDic = new Dictionary<int, AOIAreaData>();

            LoadAOIAreaData();

            PVPSceneLineDic = new Dictionary<int, PVPSceneLine>();

            //至少有一个场景线
            PVPSceneLineDic[1] = new PVPSceneLine(1, this);

            Console.WriteLine("场景{0}初始化完毕", CurrSysScene.SceneName);
        }

        /// <summary>
        /// 获取默认场景线
        /// </summary>
        public PVPSceneLine DefaultSceneLine => PVPSceneLineDic[1];
        
        /// <summary>
        /// 加载AOI数据
        /// </summary>
        private void LoadAOIAreaData()
        {
            if (!File.Exists(CurrSceneConfig.AOIJsonDataPath))
            {
                Console.WriteLine("读取AOI区域数据失败{0}", CurrSceneConfig.AOIJsonDataPath);
                return;
            }

            string json = File.ReadAllText(CurrSceneConfig.AOIJsonDataPath, Encoding.UTF8);
            CurrSceneAreaDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AOIAreaData>>(json);
            foreach (var item in CurrSceneAreaDataList)
            {
                item.Init();
                m_AOIAreaDataDic[item.AreaId] = item;
            }
        }

        
        /// <summary>
        /// 通过位置获取所在区域
        /// </summary>
        /// <param name="currPos"></param>
        /// <returns></returns>
        public int GetAOIAreaIdByPos(UnityEngine.Vector3 currPos)
        {
            foreach (var item in CurrSceneAreaDataList)
            {
                if (currPos.x >= item.TopLeftPos.x && currPos.z <= item.TopLeftPos.z
                                                   && currPos.x <= item.BottomRightPos.x &&
                                                   currPos.z >= item.BottomRightPos.z)
                {
                    return item.AreaId;
                }
            }

            return -1;
        }

        /// <summary>
        /// 获取是否可以到达
        /// </summary>
        /// <param name="currPos"></param>
        /// <returns></returns>
        public bool GetCanArrive(Vector3 currPos)
        {
            int areaId = GetAOIAreaIdByPos(currPos);
            if (areaId == -1)
            {
                return false;
            }
            
            //找到对应的区域
            AOIAreaData areaData = m_AOIAreaDataDic[areaId];
            return areaData.GetCanArrive(currPos);
        }
    }
}