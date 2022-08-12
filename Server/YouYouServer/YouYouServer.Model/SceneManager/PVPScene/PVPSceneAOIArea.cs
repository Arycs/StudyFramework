using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 场景区域
    /// </summary>
    public class PVPSceneAOIArea
    {
        /// <summary>
        /// 当前的区域数据
        /// </summary>
        public AOIAreaData CurrAOIData { get; private set; }

        public PVPSceneAOIArea(AOIAreaData aoiData)
        {
            CurrAOIData = aoiData;
        }
    }
}
