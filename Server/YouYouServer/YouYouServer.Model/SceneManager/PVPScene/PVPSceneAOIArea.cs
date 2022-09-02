using System;
using System.Collections.Generic;
using System.Text;
using YouYou;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;
using Vector3 = UnityEngine.Vector3;

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
        /// 角色列表(玩家 怪)
        /// </summary>
        public LinkedList<RoleClientBase> RoleClientList { get; private set; }

        /// <summary>
        /// 当前场景线
        /// </summary>
        public PVPSceneLine CurrSceneLine { get; private set; }

        public PVPSceneAOIArea(AOIAreaData aoiData,PVPSceneLine pvpSceneLine)
        {
            CurrAOIData = aoiData;
            CurrSceneLine = pvpSceneLine;
            RoleClientList = new LinkedList<RoleClientBase>();

            HotFixHelper.OnLoadAssembly += InitHandler;
            InitHandler();
        }

        private IPVPSceneAOIArea m_CurrHandler;

        private void InitHandler()
        {
            if (m_CurrHandler != null)
            {
                //把旧的实例释放
                m_CurrHandler.Dispose();
                m_CurrHandler = null;
            }

            m_CurrHandler =
                Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.PVPSceneAOIAreaHandler]) as
                    IPVPSceneAOIArea;
            m_CurrHandler?.Init(this);
        }

        /// <summary>
        /// tick
        /// </summary>
        public void OnUpdate()
        {
           m_CurrHandler.OnUpdate();
        }

        /// <summary>
        /// 获取这个区域和关联区域所有玩家
        /// </summary>
        /// <param name="searchRoleType"></param>
        /// <returns></returns>
        public List<RoleClientBase> GetAllRole(SearchRoleType searchRoleType)
        {
            return m_CurrHandler.GetAllRole(searchRoleType);
        }
        
        /// <summary>
        /// 角色加入场景
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void AddRole(RoleClientBase roleClientBase)
        {
            m_CurrHandler.AddRole(roleClientBase);
        }

        

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="leaveSceneLineType"></param>
        public void RemoveRole(RoleClientBase roleClientBase, LeaveSceneLineType leaveSceneLineType)
        {
            m_CurrHandler.RemoveRole(roleClientBase,leaveSceneLineType);
        }

        /// <summary>
        /// 角色移动
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="targetPos"></param>
        /// <param name="moveType"></param>
        public void RoleMove(RoleClientBase roleClientBase,  YouYou.Proto.Vector3 targetPos, PlayerActionType moveType)
        {
            m_CurrHandler.RoleMove(roleClientBase,targetPos,moveType);
        }

        /// <summary>
        /// 角色待机
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="isJoystickStop"></param>
        public void RoleIdle(RoleClientBase roleClientBase,bool isJoystickStop)
        {
            m_CurrHandler.RoleIdle(roleClientBase,isJoystickStop);
        }
        
    }
}
