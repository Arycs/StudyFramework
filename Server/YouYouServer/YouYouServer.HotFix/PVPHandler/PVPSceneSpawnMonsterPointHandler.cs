using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;
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
                MonsterClient monsterClient = new MonsterClient();
                monsterClient.CurrSpawnMonsterPoint = m_PVPSceneSpawnMonsterPoint;
                m_PVPSceneSpawnMonsterPoint.CurrMonster = monsterClient;
                m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.RoleList.AddLast(monsterClient);
                monsterClient.CurrPos = m_PVPSceneSpawnMonsterPoint.BornPos;

                monsterClient.OnDie += () => {
                    m_PVPSceneSpawnMonsterPoint.CurrMonster = null;
                    //怪死亡的时候 重新设置刷怪点的下次产卵时间

                    m_PVPSceneSpawnMonsterPoint.NextSpwanTime = TimerManager.time + m_PVPSceneSpawnMonsterPoint.interval;
                };
                monsterClient.CurrFsmManager.ChangeState(Core.RoleState.Idle);

                //设置怪的位置 ID 等信息
                //计算出怪应该放在哪个场景区域
                //把怪放到对应的场景区域
                m_PVPSceneSpawnMonsterPoint.OwnerPVPSceneLine.AOIAreaDic[1].RoleClientList.AddLast(monsterClient);
            }
        }
        public void Dispose()
        {

        }
    }
}
