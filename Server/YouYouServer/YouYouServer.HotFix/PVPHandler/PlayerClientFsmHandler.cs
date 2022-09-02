using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YouYou.Proto;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Core.Utils;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;
using YouYouServer.Model.ServerManager.Client.MonsterClient;
using Vector3 = UnityEngine.Vector3;

namespace YouYouServer.HotFix.PVPHandler
{
    [Handler(ConstDefine.PlayerClientFsmHandler)]
    public class PlayerClientFsmHandler : IRoleClientFsmHandler
    {
        public PlayerForGameClient m_PlayerForGameClient;

        private float m_Deltatime;

        public void Init(RoleClientBase roleClientBase)
        {
            m_PlayerForGameClient = roleClientBase as PlayerForGameClient;
        }

        public void OnUpdate()
        {
        }

        public void Idle_OnEnter()
        {
            Console.WriteLine("Idle_OnEnter");
        }

        public void Idle_OnLeave()
        {
        }

        public void Run_OnReSet()
        {
            Console.WriteLine("Run_OnReset");
            m_PlayerForGameClient.RunTime = 0;
            m_PlayerForGameClient.CurrWayPointIndex = 1;
            m_PlayerForGameClient.TurnComplete = false;
        }

        public void Idle_OnUpdate()
        {
        }

        public void Run_OnEnter()
        {
            Console.WriteLine("Run_OnEnter");
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_PlayerForGameClient.CurrSceneId,
                out var pvpScene))
            {
                m_Deltatime = pvpScene.DefaultSceneLine.Deltatime;
            }

            if (m_PlayerForGameClient.MoveType == PlayerActionType.JoystickMove)
            {
                return;
            }

            m_PlayerForGameClient.RunTime = 0;
            m_PlayerForGameClient.CurrWayPointIndex = 1;
            m_PlayerForGameClient.CurrPos = m_PlayerForGameClient.PathPoints[0];
            m_PlayerForGameClient.TurnComplete = false;

            //计算移动的距离
            m_PlayerForGameClient.MoveDis = GameUtil.GetPathLen(m_PlayerForGameClient.PathPoints);
            //距离/速度=到达所需时间（秒）
            m_PlayerForGameClient.RunNeedTime = m_PlayerForGameClient.MoveDis / m_PlayerForGameClient.RunSpeed;
            m_PlayerForGameClient.RunNeedTime -= m_PlayerForGameClient.TotalPingValue * 0.001f;

            //修正速度
            m_PlayerForGameClient.ModifyRunSpeed = m_PlayerForGameClient.MoveDis / m_PlayerForGameClient.RunNeedTime;

            Console.WriteLine("RoleId = {0} ModifyRunSpeed = {1}", m_PlayerForGameClient.RoleId,
                m_PlayerForGameClient.ModifyRunSpeed);

            m_PlayerForGameClient.ModifyRunSpeed = Mathf.Clamp(m_PlayerForGameClient.ModifyRunSpeed, 10, 15);
        }

        public void Run_OnLeave()
        {
        }

        public void Run_OnUpdate()
        {
            if (m_PlayerForGameClient.MoveType == PlayerActionType.JoystickMove)
            {
                JoystickMove();
            }
            else
            {
                ClickMove();
            }
        }

        /// <summary>
        /// 摇杆移动
        /// </summary>
        private void JoystickMove()
        {
            m_PlayerForGameClient.RunEndPos = m_PlayerForGameClient.TargetPos;
            m_PlayerForGameClient.RunBeginPos = m_PlayerForGameClient.CurrPos;

            Console.WriteLine("JoystickMove CurrPos 001 " + m_PlayerForGameClient.CurrPos);
            Vector3 dir = (m_PlayerForGameClient.RunEndPos - m_PlayerForGameClient.RunBeginPos).normalized;

            //移动方向
            m_PlayerForGameClient.RunDir = dir;

            Console.WriteLine("JoystickMove RunDir 01 " + m_PlayerForGameClient.RunDir);
            Console.WriteLine("JoystickMove RunEndPos 001 " + m_PlayerForGameClient.RunEndPos);

            //====================================================================================================
            //距离/速度=到达所需时间（秒）
            float dis = m_PlayerForGameClient.RunDir.magnitude;
            m_PlayerForGameClient.RunNeedTime = dis / m_PlayerForGameClient.RunSpeed;
            m_PlayerForGameClient.RunNeedTime -= m_PlayerForGameClient.TotalPingValue * 0.001f;

            //修正速度
            m_PlayerForGameClient.ModifyRunSpeed = dis / m_PlayerForGameClient.RunNeedTime;

            m_PlayerForGameClient.ModifyRunSpeed = Mathf.Clamp(m_PlayerForGameClient.ModifyRunSpeed, 10, 15);

            Console.WriteLine("RoleId = {0} ModifyRunSpeed = {1} TotalPingValue = {2} dis = {3} RunNeedTime = {4}",
                m_PlayerForGameClient.RoleId,
                m_PlayerForGameClient.ModifyRunSpeed, m_PlayerForGameClient.TotalPingValue, dis,
                m_PlayerForGameClient.RunNeedTime);
            //====================================================================================================


            //一帧移动的距离
            m_PlayerForGameClient.RunDir =
                m_PlayerForGameClient.RunDir * m_Deltatime * m_PlayerForGameClient.ModifyRunSpeed;
            Console.WriteLine("JoystickMove m_Deltatime " + m_Deltatime);
            Console.WriteLine("JoystickMove ModifyRunSpeed " + m_PlayerForGameClient.ModifyRunSpeed);
            Console.WriteLine("JoystickMove RunDir 02 " + m_PlayerForGameClient.RunDir);

            //修改当前位置
            m_PlayerForGameClient.CurrPos = m_PlayerForGameClient.CurrPos + m_PlayerForGameClient.RunDir;

            Console.WriteLine("JoystickMove CurrPos 002 " + m_PlayerForGameClient.CurrPos);

            //如果角色移动过头了 方向会不相等 那么不重新计算y轴
            if ((m_PlayerForGameClient.RunEndPos - m_PlayerForGameClient.CurrPos).normalized == dir)
            {
                //转身
                float y = (float) Math.Atan2((m_PlayerForGameClient.RunEndPos.x - m_PlayerForGameClient.CurrPos.x),
                    (m_PlayerForGameClient.RunEndPos.z - m_PlayerForGameClient.CurrPos.z)) * 180 / (float) Math.PI;

                m_PlayerForGameClient.CurrRotationY = y;

                Console.WriteLine("JoystickMove y " + y);
            }

            //写入DB数据
            m_PlayerForGameClient.CurrRole.PosData = new YouYou.Proto.Vector3
            {
                X = m_PlayerForGameClient.CurrPos.x, Y = m_PlayerForGameClient.CurrPos.y,
                Z = m_PlayerForGameClient.CurrPos.z
            };
            m_PlayerForGameClient.CurrRole.RotationY = m_PlayerForGameClient.CurrRotationY;
        }

        /// <summary>
        /// 点击移动
        /// </summary>
        private void ClickMove()
        {
            m_PlayerForGameClient.RunTime += m_Deltatime;
            if (m_PlayerForGameClient.CurrWayPointIndex == m_PlayerForGameClient.PathPoints.Count)
            {
                Console.WriteLine("Run_OnUpdate ChangeState To Idle Role = {0}", m_PlayerForGameClient.RoleId);
                m_PlayerForGameClient.CurrFsmManager.ChangeState(Core.RoleState.Idle);
                return;
            }

            if (!m_PlayerForGameClient.TurnComplete)
            {
                m_PlayerForGameClient.RunEndPos =
                    m_PlayerForGameClient.PathPoints[m_PlayerForGameClient.CurrWayPointIndex];
                m_PlayerForGameClient.RunBeginPos =
                    m_PlayerForGameClient.PathPoints[m_PlayerForGameClient.CurrWayPointIndex - 1];
                m_PlayerForGameClient.RunDir =
                    (m_PlayerForGameClient.RunEndPos - m_PlayerForGameClient.RunBeginPos).normalized;

                float y = (float) Math.Atan2((m_PlayerForGameClient.RunEndPos.x - m_PlayerForGameClient.RunBeginPos.x),
                    (m_PlayerForGameClient.RunEndPos.z - m_PlayerForGameClient.RunBeginPos.z)) * 180 / (float) Math.PI;
                m_PlayerForGameClient.CurrRotationY = y;
                m_PlayerForGameClient.TurnComplete = true;

                // Console.WriteLine("Run_OnUpdate Role = {0} Turn RunBeginPos = {1} RunEndPos = {2}", m_MonsterClient.RoleId,
                //     m_MonsterClient.RunBeginPos,
                //     m_MonsterClient.RunEndPos);
            }

            //时间*速度=距离
            float dis = m_PlayerForGameClient.RunTime * m_PlayerForGameClient.ModifyRunSpeed;
            m_PlayerForGameClient.CurrPos = m_PlayerForGameClient.RunBeginPos + m_PlayerForGameClient.RunDir * dis;

            //写入DB数据
            m_PlayerForGameClient.CurrRole.PosData = new YouYou.Proto.Vector3
            {
                X = m_PlayerForGameClient.CurrPos.x, Y = m_PlayerForGameClient.CurrPos.y,
                Z = m_PlayerForGameClient.CurrPos.z
            };
            m_PlayerForGameClient.CurrRole.RotationY = m_PlayerForGameClient.CurrRotationY;

            if (dis >= UnityEngine.Vector3.Distance(m_PlayerForGameClient.RunEndPos, m_PlayerForGameClient.RunBeginPos))
            {
                m_PlayerForGameClient.CurrPos = m_PlayerForGameClient.RunEndPos; //位置修正
                m_PlayerForGameClient.RunTime = 0;
                m_PlayerForGameClient.TurnComplete = false;
                m_PlayerForGameClient.CurrWayPointIndex++;
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