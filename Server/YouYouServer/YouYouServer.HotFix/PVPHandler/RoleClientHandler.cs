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
        public float m_EnterIdleTime;

        public MonsterClient m_MonsterClient;

        public void Init(RoleClientBase roleClientBase)
        {
            m_MonsterClient = (MonsterClient)roleClientBase;
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
                Vector3 targetPos = m_MonsterClient.CurrSpawnMonsterPoint.PatrolPosList[new System.Random().Next(0, m_MonsterClient.CurrSpawnMonsterPoint.PatrolPosList.Count)];

                m_MonsterClient.TargetPos = targetPos;

                GameServerManager.ConnectNavAgent.GetNavPath(m_MonsterClient.CurrSceneId, m_MonsterClient.CurrPos, m_MonsterClient.TargetPos, (NS2GS_ReturnNavPath proto) =>
                 {
                     Console.WriteLine(proto.TaskId);
                     Console.WriteLine(proto.Valid);
                     Console.WriteLine(proto.Path);

                 });
            }
        }

        public void OnUpdate()
        {
        }

        public void Run_OnEnter()
        {
            Console.WriteLine("Run_OnEnter");
        }

        public void Run_OnLeave()
        {
        }

        public void Run_OnUpdate()
        {
        }
    }
}
