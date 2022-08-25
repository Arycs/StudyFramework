using System.Collections.Generic;
using YouYou.Proto;
using YouYouServer.Core;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.Model.IHandler
{
    /// <summary>
    /// 场景区域处理句柄
    /// </summary>
    public interface IPVPSceneAOIArea
    {
        void Init(PVPSceneAOIArea pvpSceneAoiArea);

        void OnUpdate();

        void Dispose();

        List<RoleClientBase> GetAllRole(SearchRoleType searchRoleType);

        void AddRole(RoleClientBase roleClientBase);

        void RoleEnterSceneLine(RoleClientBase roleClient, PlayerForGameClient otherPlayer);

        void RemoveRole(RoleClientBase roleClientBase, LeaveSceneLineType leaveSceneLineType);

        /// <summary>
        /// 角色移动
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="targetPos"></param>
        void RoleMove(RoleClientBase roleClientBase, Vector3 targetPos);

        /// <summary>
        /// 角色待机
        /// </summary>
        /// <param name="roleClientBase"></param>
        void RoleIdle(RoleClientBase roleClientBase);

        /// <summary>
        /// 检查角色跨区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        void CheckAreaChange(RoleClientBase roleClientBase);
    }
}