using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core;
using YouYouServer.Model.RoleFsm.RoleFsmState;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.Model.RoleFsm
{
    /// <summary>
    /// 角色状态机管理器
    /// </summary>
    public class RoleFsm
    {
        public RoleClientBase CurrRoleClient { get; private set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        private RoleFsmStateBase m_CurrState;

        /// <summary>
        /// 当前角色状态枚举
        /// </summary>
        public RoleState CurrStateType;

        /// <summary>
        /// 状态字典
        /// </summary>
        private Dictionary<RoleState, RoleFsmStateBase> m_StateDic;

        public RoleFsmRun RoleFsmRun { get; private set; }
        
        public RoleFsm(RoleClientBase roleClientBase)
        {
            CurrRoleClient = roleClientBase;
            m_StateDic = new Dictionary<RoleState, RoleFsmStateBase>();

            RoleFsmRun = new RoleFsmRun(this);
            m_StateDic[RoleState.Idle] = new RoleFsmIdle(this);
            m_StateDic[RoleState.Run] = RoleFsmRun;
            m_StateDic[RoleState.Attack] = new RoleFsmAttack(this);
            m_StateDic[RoleState.Hurt] = new RoleFsmHurt(this);
            m_StateDic[RoleState.Die] = new RoleFsmDie(this);
        }

        public void OnUpdate()
        {
            if (m_CurrState == null)
            {
                return;
            }
            m_CurrState.OnUpdate();
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeState(RoleState newState)
        {
            // 两个状态一样 不重复进入
            if (CurrStateType == newState)
            {
                return;
            }

            if (m_CurrState != null)
            {
                m_CurrState.OnLeave();
            }

            CurrStateType = newState;
            m_CurrState = m_StateDic[CurrStateType];

            //进入新状态
            m_CurrState.OnEnter();
        }

    }

}
