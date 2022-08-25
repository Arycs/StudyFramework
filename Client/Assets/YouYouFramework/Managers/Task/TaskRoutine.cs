﻿namespace YouYou
{
    /// <summary>
    /// 任务执行器
    /// </summary>
    public class TaskRoutine
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int TaskRoutineId;

        /// <summary>
        /// 具体的任务
        /// </summary>
        /// <returns></returns>
        public BaseAction CurrTask;

        /// <summary>
        /// 任务完成
        /// </summary>
        public BaseAction OnComplete;

        /// <summary>
        /// 停止任务
        /// </summary>
        public BaseAction StopTask;
        
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool Complete { get;private set; }

        /// <summary>
        /// 任务数据
        /// </summary>
        public object TaskData;

        /// <summary>
        /// 进入任务
        /// </summary>
        public void Enter()
        {
            Complete = false;

            if (CurrTask != null)
            {
                CurrTask.Invoke();
            }
            else
            {
                Leave();
            }
        }

        public void OnUpdate()
        {
            if (Complete)
            {
                OnComplete ?.Invoke();
                CurrTask = null;
                OnComplete = null;
                Complete = false;
                GameEntry.Pool.EnqueueClassObject(this);
            }
        }

        public void Leave()
        {
            Complete = true;
        }

    }
}