using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager;

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

        /// <summary>
        /// 角色列表
        /// </summary>
        public LinkedList<RoleClientBase> RoleClientList { get; private set; }

        public PVPSceneAOIArea(AOIAreaData aoiData)
        {
            CurrAOIData = aoiData;
            RoleClientList = new LinkedList<RoleClientBase>();
        }

        public void OnUpdate()
        {
            LinkedListNode<RoleClientBase> curr = RoleClientList.First;
            while (curr != null)
            {
                LinkedListNode<RoleClientBase> next = curr.Next;
                curr.Value.OnUpdate();
                curr = next;
            }
        
        }

    }
}
