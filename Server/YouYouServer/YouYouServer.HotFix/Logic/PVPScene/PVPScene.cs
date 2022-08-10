using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Model.DataTable;

namespace YouYouServer.HotFix.Logic.PVPScene
{
    /// <summary>
    /// PVP场景
    /// </summary>
    public class PVPScene
    {
        /// <summary>
        /// 场景编号
        /// </summary>
        public int SceneId { get; }

        /// <summary>
        /// 当前场景表格数据
        /// </summary>
        public DTSys_SceneEntity CurrSysScene { get; }

        /// <summary>
        /// 场景线字典
        /// </summary>
        public Dictionary<int, PVPSceneLine> PVPSceneLineDic;

        public PVPScene(int sceneId)
        {
            SceneId = sceneId;
            CurrSysScene = DataTableManager.Sys_SceneList.GetDic(SceneId);

            PVPSceneLineDic = new Dictionary<int, PVPSceneLine>();

            //至少有一个场景线
            PVPSceneLineDic[1] = new PVPSceneLine(1, this);

            Console.WriteLine("场景{0}初始化完毕", CurrSysScene.SceneName);
        }
    }
}
