using System;
using System.Collections.Generic;
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
    [Handler(ConstDefine.MonsterClientFsmHandler)]
    public class MonsterClientFsmHandler : IRoleClientFsmHandler
    {
        private MonsterClient m_MonsterClient;

        public void Init(RoleClientBase roleClientBase)
        {
            m_MonsterClient = roleClientBase as MonsterClient;
        }

        public void OnUpdate()
        {
        }

        public void Idle_OnEnter()
        {
            m_MonsterClient.IsPatrol = false;
            Console.WriteLine("Idle_OnEnter" + DateTime.Now);
            m_MonsterClient.EnterIdleTime = TimerManager.time;
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_MonsterClient.CurrSceneId,
                out var pvpScene))
            {
                pvpScene.DefaultSceneLine.AOIAreaDic[m_MonsterClient.CurrAreaId].RoleIdle(m_MonsterClient);
            }
        }

        public void Idle_OnLeave()
        {
        }

        public void Idle_OnUpdate()
        {
            //待机 10 秒巡逻
            if (TimerManager.time > m_MonsterClient.EnterIdleTime + 60)
            {
                if (m_MonsterClient.IsPatrol)
                {
                    return;
                }

                m_MonsterClient.EnterIdleTime = TimerManager.time;
                m_MonsterClient.IsPatrol = true; //此处一定设置为巡逻中 防止重复进入

                //随机找一个巡逻点
                int len = m_MonsterClient.CurrSpawnMonsterPoint.PatrolPosList.Count;
                List<int> indexList = new List<int>(10);

                for (int i = 0; i < len; i++)
                {
                    if (i == m_MonsterClient.PrevPatrolPosIndex)
                    {
                        //排除上一个索引
                        continue;
                    }

                    indexList.Add(i);
                }

                //找到一个随机的索引
                m_MonsterClient.PrevPatrolPosIndex = indexList[new System.Random().Next(0, indexList.Count)];
                UnityEngine.Vector3 targetPos =
                    m_MonsterClient.CurrSpawnMonsterPoint.PatrolPosList[m_MonsterClient.PrevPatrolPosIndex];

                m_MonsterClient.TargetPos = targetPos;
                Console.WriteLine(
                    $"GetNavPath CurrPos = {m_MonsterClient.CurrPos},TargetPos = {m_MonsterClient.TargetPos}");

                //进行寻路
                GameServerManager.ConnectNavAgent.GetNavPath(m_MonsterClient.CurrSceneId, m_MonsterClient.CurrPos,
                    m_MonsterClient.TargetPos, (NS2GS_ReturnNavPath proto) =>
                    {
                        if (proto.Path.Count > 1)
                        {
                            m_MonsterClient.PathPoints.Clear();
                            foreach (var item in proto.Path)
                            {
                                m_MonsterClient.PathPoints.Add(new UnityEngine.Vector3(item.X, item.Y, item.Z));
                            }

                            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_MonsterClient.CurrSceneId,
                                out var pvpScene))
                            {
                                pvpScene.DefaultSceneLine.AOIAreaDic[m_MonsterClient.CurrAreaId].RoleMove(
                                    m_MonsterClient,
                                    new Vector3() {X = targetPos.x, Y = targetPos.y, Z = targetPos.z});
                            }

                            m_MonsterClient.CurrFsmManager.ChangeState(RoleState.Run);
                        }
                        else
                        {
                            m_MonsterClient.IsPatrol = false;
                        }
                    });
            }
        }

        public void Run_OnEnter()
        {
            Console.WriteLine("Run_OnEnter");
            m_MonsterClient.RunTime = 0;
            m_MonsterClient.CurrWayPointIndex = 1;
            m_MonsterClient.CurrPos = m_MonsterClient.PathPoints[0];
            m_MonsterClient.TurnComplete = false;
        }

        public void Run_OnLeave()
        {
        }

        public void Run_OnUpdate()
        {
            m_MonsterClient.RunTime += m_MonsterClient.CurrSpawnMonsterPoint.OwnerPVPSceneLine.Deltatime;
            if (m_MonsterClient.CurrWayPointIndex == m_MonsterClient.PathPoints.Count)
            {
                m_MonsterClient.CurrFsmManager.ChangeState(RoleState.Idle);
                return;
            }

            if (!m_MonsterClient.TurnComplete)
            {
                m_MonsterClient.RunEndPos = m_MonsterClient.PathPoints[m_MonsterClient.CurrWayPointIndex];
                m_MonsterClient.RunBeginPos = m_MonsterClient.PathPoints[m_MonsterClient.CurrWayPointIndex - 1];
                m_MonsterClient.RunDir = (m_MonsterClient.RunEndPos - m_MonsterClient.RunBeginPos).normalized;

                float y = (float) Math.Atan2((m_MonsterClient.RunEndPos.x - m_MonsterClient.RunBeginPos.x),
                    (m_MonsterClient.RunEndPos.z - m_MonsterClient.RunBeginPos.z)) * 180 / (float) Math.PI;
                m_MonsterClient.CurrRotationY = y;
                m_MonsterClient.TurnComplete = true;

                Console.WriteLine(
                    $"Role Turn RunBeginPos = {m_MonsterClient.RunBeginPos} RunEndPos= {m_MonsterClient.RunEndPos}");
            }

            //时间 * 速度 = 距离
            float dis = m_MonsterClient.RunTime * m_MonsterClient.RunSpeed;
            m_MonsterClient.CurrPos = m_MonsterClient.RunBeginPos + m_MonsterClient.RunDir * dis;

            //TODO 这里每个怪 每帧都在检查是否跨区域, 不太好,需要优化
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_MonsterClient.CurrSceneId,
                out var pvpScene))
            {
                pvpScene.DefaultSceneLine.AOIAreaDic[m_MonsterClient.CurrAreaId].CheckAreaChange(m_MonsterClient);
            }

            if (dis >= UnityEngine.Vector3.Distance(m_MonsterClient.RunEndPos, m_MonsterClient.RunBeginPos))
            {
                m_MonsterClient.CurrPos = m_MonsterClient.RunEndPos; //位置修正
                m_MonsterClient.RunTime = 0;
                m_MonsterClient.TurnComplete = false;
                m_MonsterClient.CurrWayPointIndex++;
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
            Console.WriteLine("Die_OnEnter" + DateTime.Now);
            m_MonsterClient.OnDie?.Invoke();
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