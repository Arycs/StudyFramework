using System;
using System.Collections.Generic;
using System.Text;
using YouYou.Proto;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Core.Utils;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.SceneManager.PVPScene;
using YouYouServer.Model.ServerManager.Client.MonsterClient;

namespace YouYouServer.HotFix.PVPHandler
{
    [Handler(ConstDefine.PVPSceneSpawnMonsterPointHandler)]
    public class PVPSceneSpawnMonsterPointHandler : IPVPSceneSpawnMonsterPointHandler
    {
        private PVPSceneSpawnMonsterPoint m_PVPSceneSpawnMonsterPoint;

        public void Init(PVPSceneSpawnMonsterPoint pvpSceneSpawnMonsterPoint)
        {
            m_PVPSceneSpawnMonsterPoint = pvpSceneSpawnMonsterPoint;
        }

        public void OnUpdate()
        {
            if (m_PVPSceneSpawnMonsterPoint.CurrMonster == null
                && TimerManager.time >= m_PVPSceneSpawnMonsterPoint.NextSpwanTime)
            {
                //刷怪
                MonsterClient monsterClient = new MonsterClient
                {
                    RoleId = UniqueIdUtil.GetUniqueId(),
                    CurrSpawnMonsterPoint = m_PVPSceneSpawnMonsterPoint,
                    CurrPos = m_PVPSceneSpawnMonsterPoint.BornPos
                };

                //设置怪的位置 ID 等信息


                monsterClient.OnDie += () =>
                {
                    //从场景线中移除
                    m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.RoleList.Remove(monsterClient);
                    Console.WriteLine("从场景线中移除 场景线角色数量=" +
                                      m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.RoleList.Count);

                    //从区域中移除
                    m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.AOIAreaDic[monsterClient.CurrAreaId]
                        .RemoveRole(monsterClient, LeaveSceneLineType.Die);
                    Console.WriteLine("从场景线区域中移除 场景线区域角色数量=" + m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine
                        .AOIAreaDic[monsterClient.CurrAreaId].RoleClientList.Count);

                    m_PVPSceneSpawnMonsterPoint.CurrMonster = null;

                    //怪死亡的时候 重新设置刷怪点的下次产卵时间
                    m_PVPSceneSpawnMonsterPoint.NextSpwanTime =
                        TimerManager.time + m_PVPSceneSpawnMonsterPoint.interval;
                };

                m_PVPSceneSpawnMonsterPoint.CurrMonster = monsterClient;
                m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.RoleList.AddLast(monsterClient);

                //计算出怪应该放在哪个场景区域
                //把怪放到对应的场景区域
                int areaId =
                    m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.OwnerPVPScene
                        .GetAOIAreaIdByPos(monsterClient.CurrPos);
                if (areaId > 0)
                {
                    Console.WriteLine("刷新怪 RoleId=" + monsterClient.RoleId + "areaId=" + areaId);
                    m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.AOIAreaDic[areaId].AddRole(monsterClient);
                    monsterClient.CurrAreaId = areaId; //设置当前区域编号
                    monsterClient.CurrFsmManager.ChangeState(Core.RoleState.Idle);
                }
                else
                {
                    Console.WriteLine("areaId 不存在");
                }
            }
        }

        public void Dispose()
        {
        }
    }
}