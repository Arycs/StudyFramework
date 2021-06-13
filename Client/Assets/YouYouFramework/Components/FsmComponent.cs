using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// 状态机组件
    /// </summary>
    public class FsmComponent : YouYouBaseComponent
    {
        /// <summary>
        /// 状态机管理器
        /// </summary>
        public FsmManager m_FsmManager;

        /// <summary>
        /// 状态机的临时编号
        /// </summary>
        public int m_TempFsmId = 0;
        
        protected override void OnAwake()
        {
            base.OnAwake();
            m_FsmManager = new FsmManager();
        }

        #region Create创建状态机
        public Fsm<T> Create<T>(T owner, FsmState<T>[] states) where T : class
        {
            return m_FsmManager.Create<T>(m_TempFsmId++, owner, states);
        }
        #endregion

        #region Destroy销毁状态机
        public void DestroyFsm(int fsmId)
        {
            m_FsmManager.DestroyFsm(fsmId);
        }
        #endregion
       
        
        public override void Shutdown()
        {
            m_FsmManager.Dispose();
        }

        public void OnUpdate()
        {
            
        }
    }
}
