using System;
using System.Collections.Generic;
using System.Linq;

namespace YouYou
{
    /// <summary>
    /// 任务组
    /// </summary>
    public class TaskGroup : IDisposable
    {
        /// <summary>
        /// 任务列表
        /// </summary>
        private LinkedList<TaskRoutine> m_TaskRoutineList;

        /// <summary>
        /// 任务组完成
        /// </summary>
        public BaseAction OnComplete;

        /// <summary>
        /// 是否并发执行
        /// </summary>
        private bool m_IsConcurrency = false;

        public TaskGroup()
        {
            m_TaskRoutineList = new LinkedList<TaskRoutine>();
        }

        public void Dispose()
        {
            m_TaskRoutineList.Clear();
            OnComplete = null;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="routine"></param>
        public void AddTask(TaskRoutine routine)
        {
            m_TaskRoutineList.AddLast(routine);
        }

        /// <summary>
        /// 清空所有任务
        /// </summary>
        public void CreateAllTask()
        {
            LinkedListNode<TaskRoutine> routine = m_TaskRoutineList.First;
            while (routine != null)
            {
                var next = routine.Next;
                routine.Value.StopTask?.Invoke();
                GameEntry.Pool.EnqueueClassObject(routine);
                m_TaskRoutineList.Remove(routine);
                routine = next;
            }
        }

        public void OnUpdate()
        {
            var routine = m_TaskRoutineList.First;
            while (routine !=null)
            {
                routine.Value.OnUpdate();
                routine = routine.Next;
            }
        }

        public void Run(bool isConcurrency = false)
        {
            m_IsConcurrency = isConcurrency;
            if (m_IsConcurrency)
            {
                ConcurrencyTask();
            }
            else
            {
                //按顺序执行任务
                CheckTask();
            }
        }

        /// <summary>
        /// 检查任务, 按顺序执行
        /// </summary>
        private void CheckTask()
        {
            var curr = m_TaskRoutineList.First;
            if (curr != null)
            {
                curr.Value.OnComplete = () =>
                {
                    m_TaskRoutineList.Remove(curr);
                    CheckTask();
                };
                curr.Value.Enter();
            }
            else
            {
                OnComplete?.Invoke();
                Dispose();
                GameEntry.Task.RemoveTaskGroup(this);
                GameEntry.Pool.EnqueueClassObject(this);
            }
        }

        private int m_TotalCount = 0;
        private int m_CurrCount = 0;

        /// <summary>
        /// 并发执行任务
        /// </summary>
        private void ConcurrencyTask()
        {
            
        }
    }
}