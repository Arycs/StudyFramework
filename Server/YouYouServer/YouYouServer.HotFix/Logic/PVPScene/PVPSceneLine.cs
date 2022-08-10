using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.HotFix.Logic.PVPScene
{
    /// <summary>
    /// 场景线
    /// </summary>
    public class PVPSceneLine
    {
        /// <summary>
        /// 场景线编号
        /// </summary>
        public int PVPSceneLineId { get; }

        /// <summary>
        /// 所属场景
        /// </summary>
        public PVPScene OwnerPVPScene { get; }

        public PVPSceneLine(int pvpSceneLineId, PVPScene ownerPVPScene)
        {
            PVPSceneLineId = pvpSceneLineId;
            OwnerPVPScene = ownerPVPScene;

            InitSceneAOIArea();
        }

        /// <summary>
        /// 初始化场景AOI区域
        /// </summary>
        private void InitSceneAOIArea()
        {
            Console.WriteLine("初始化场景 {0} AOI区域", OwnerPVPScene.CurrSysScene.SceneName);
        }
    }
}
