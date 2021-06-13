using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YouYou
{
    /// <summary>
    /// 时间组件
    /// </summary>
    public class TimeComponent : YouYouBaseComponent,IUpdateComponent
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            GameEntry.RegisterUpdateComponent(this);
            m_TimeManager = new TimeManager();
        }

        #region 定时器相关
        /// <summary>
        /// 定时器管理器
        /// </summary>
        public TimeManager m_TimeManager;
        
        /// <summary>
        /// 注册定时器
        /// </summary>
        /// <param name="action"></param>
        internal void RegisterTimeAction(TimeAction action)
        {
            m_TimeManager.RegisterTimeAction(action);
        }
        
        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="action"></param>
        internal void RemoveTimeAction(TimeAction action)
        {
            m_TimeManager.RemoveTimeAction(action);
            GameEntry.Pool.EnqueueClassObject(action);
        }

        /// <summary>
        /// 创建定时器
        /// </summary>
        /// <returns></returns>
        public TimeAction CreateTimeAction()
        {
            return GameEntry.Pool.DequeueClassObject<TimeAction>();
        }
        #endregion
        public override void Shutdown()
        {
            m_TimeManager.Dispose();
        }

        public void OnUpdate()
        {
            //Debug.Log("时间组件OnUpdate");
            m_TimeManager.OnUpdate();
        }

        #region  Yield等一帧
        public void Yield(BaseAction onComplete)
        {
            StartCoroutine(YieldCoroutine(onComplete));
        }

        private IEnumerator YieldCoroutine(BaseAction onComplete)
        {
            yield return null;
            if (onComplete != null)
            {
                onComplete();
            }
        }
        #endregion

    }
}

