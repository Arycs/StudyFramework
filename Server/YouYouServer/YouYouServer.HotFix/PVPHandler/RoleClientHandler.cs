using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YouYou.Proto;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;
using YouYouServer.Model.ServerManager.Client.MonsterClient;

namespace YouYouServer.HotFix.PVPHandler
{
    [Handler(ConstDefine.RoleClientHandler)]
    public class RoleClientHandler : IRoleClientHandler
    {
        public MonsterClient m_MonsterClient;

        public void Init(RoleClientBase roleClientBase)
        {
            m_MonsterClient = roleClientBase as MonsterClient;
        }

        public void OnUpdate()
        {

        }

        /// <summary>
        /// 路径点集合
        /// </summary>
        private List<Vector3> m_PathPoints = new List<Vector3>();

        private float m_EnterIdleTime = 0;

        public void Idle_OnEnter()
        {
            Console.WriteLine("Idle_OnEnter");
            m_EnterIdleTime = TimerManager.time;
        }

        public void Idle_OnLeave()
        {
        }

        public void Idle_OnUpdate()
        {
            //待机 60 秒巡逻
            if (TimerManager.time > m_EnterIdleTime + 10)
            {
                m_EnterIdleTime = TimerManager.time;
                //随机找一个巡逻点
                //Vector3 targetPos = m_MonsterClient.CurrSpawnMonsterPoint.PatrolPosList[new System.Random().Next(0, m_MonsterClient.CurrSpawnMonsterPoint.PatrolPosList.Count)];

                //m_MonsterClient.TargetPos = targetPos;

                //GameServerManager.ConnectNavAgent.GetNavPath(m_MonsterClient.CurrSceneId, m_MonsterClient.CurrPos, m_MonsterClient.TargetPos, (NS2GS_ReturnNavPath proto) =>
                // {
                //     Console.WriteLine(proto.TaskId);
                //     Console.WriteLine(proto.Valid);
                //     Console.WriteLine(proto.Path);

                // });

                m_PathPoints.Clear();
                m_PathPoints.Add(new Vector3(-15.3f, -19.2f, 23.3f));

                m_MonsterClient.CurrFsmManager.ChangeState(RoleState.Run);
            }
        }

        private float m_Speed = 10f;
        private float runTime = 0f;

        /// <summary>
        /// 当前路径点索引
        /// </summary>
        private int CurrWayPointIndex = 0;

        private float m_BeginTime = 0;
        private bool m_TurnComplete = false; //转身完毕标志
        private bool sample = false; //采样标记, 实际无意义

        private Vector3 endPos;
        private Vector3 beginPos;

        private Vector3 dir;
        private Vector3 currPos;
        public void Run_OnEnter()
        {
            Console.WriteLine("Run_OnEnter");
            runTime = 0;
            CurrWayPointIndex = 1;
            m_MonsterClient.CurrPos = m_PathPoints[0];
            m_BeginTime = TimerManager.time;
            m_TurnComplete = false;
            sample = false;
        }

        public void Run_OnLeave()
        {
        }

        public void Run_OnUpdate()
        {
            runTime += m_MonsterClient.CurrSpawnMonsterPoint.OwnerPVPSceneLine.Deltatime;
            if (CurrWayPointIndex == m_PathPoints.Count)
            {
                Console.WriteLine("走路完毕 耗时 " + (TimerManager.time - m_BeginTime));
                m_MonsterClient.CurrFsmManager.ChangeState(RoleState.Idle);
                return;
            }

            if (sample == false && TimerManager.time - m_BeginTime > 3)
            {
                sample = true;
            }

            if (!m_TurnComplete)
            {
                endPos = m_PathPoints[CurrWayPointIndex];
                beginPos = m_PathPoints[CurrWayPointIndex - 1];
                dir = (endPos - beginPos).normalized;

                float y = (float)Math.Atan2((endPos.x - beginPos.x), (endPos.z - beginPos.z)) * 180 / (float)Math.PI;
                m_MonsterClient.CurrRotationY = y;
                m_TurnComplete = true;
            }


            //时间 * 速度 = 距离
            float dis = runTime * m_Speed;
            currPos = beginPos + dir * dis;
            m_MonsterClient.CurrPos = currPos;

            if (dis >= Vector3.Distance(endPos, beginPos))
            {
                m_MonsterClient.CurrPos = endPos; //位置修正
                runTime = 0;
                m_TurnComplete = false;
                CurrWayPointIndex++;
            }
        }

        public void Attack_OnEnter()
        {
        }

        public void Attack_OnLeave()
        {
        }

        public void Attack_OnUpdate()
        {
        }

        public void Die_OnEnter()
        {
        }

        public void Die_OnLeave()
        {
        }

        public void Die_OnUpdate()
        {
        }

        public void Dispose()
        {
        }

        public void Hurt_OnEnter()
        {
        }

        public void Hurt_OnLeave()
        {
        }

        public void Hurt_OnUpdate()
        {
        }

    }
}
