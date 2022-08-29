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
        
        /// <summary>
        /// 角色进入场景线
        /// </summary>
        /// <param name="roleClient"></param>
        /// <param name="otherPlayer"></param>
        void RoleEnterSceneLine(RoleClientBase roleClient, PlayerForGameClient otherPlayer);
        
        /// <summary>
        /// 检查角色跨区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        void CheckAreaChange(RoleClientBase roleClientBase);
        
        /// <summary>
        /// 角色进入新区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="areaId"></param>
        void RoleEnterNewArea(RoleClientBase roleClientBase, int areaId);
    }
}
