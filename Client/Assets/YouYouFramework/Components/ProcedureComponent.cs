using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYou
{
    public class ProcedureComponent : YouYouBaseComponent, IUpdateComponent
    {
        /// <summary>
        /// 流程管理器
        /// </summary>
        private ProcedureManager m_ProcedureManager;

        public ProcedureState currProcedureState
        {
            get { return m_ProcedureManager.CurProcedureState; }
        }

        public FsmState<ProcedureManager> CurrProcedure
        {
            get { return m_ProcedureManager.CurrProcedure; }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            m_ProcedureManager = new ProcedureManager();
        }

        protected override void OnStart()
        {
            base.OnStart();

            //要在Start时候进行初始化
            m_ProcedureManager.Init();
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TData">泛型类型</typeparam>
        public void SetData<TData>(string key, TData value)
        {
            m_ProcedureManager.CurrFsm.SetData<TData>(key, value);
        }

        /// <summary>
        /// 取得参数
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        public TData GetData<TData>(string key)
        {
            return m_ProcedureManager.CurrFsm.GetData<TData>(key);
        }


        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(ProcedureState state)
        {
            m_ProcedureManager.ChangeState(state);
        }

        public void OnUpdate()
        {
            m_ProcedureManager.OnUpdate();
        }

        public override void Shutdown()
        {
            m_ProcedureManager.Dispose();
        }
    }
}