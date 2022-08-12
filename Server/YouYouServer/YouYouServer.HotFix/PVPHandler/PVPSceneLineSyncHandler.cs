using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;
using YouYouServer.Model.ServerManager.Client.MonsterClient;

namespace YouYouServer.HotFix.PVPHandler
{
    [Handler(ConstDefine.PVPSceneLineSyncHandler)]
    public class PVPSceneLineSyncHandler : IPVPSceneLineSyncHandler
    {
        private PVPSceneLine m_CurrPVPSceneLine;

        public void Init(PVPSceneLine pvpSceneLine)
        {
            m_CurrPVPSceneLine = pvpSceneLine;
        }

        public void SyncStatus()
        {
            Thread.Sleep(20);
            //具体逻辑在这里写
            foreach (var item in m_CurrPVPSceneLine.AOIAreaDic)
            {
                item.Value.OnUpdate();
            }

            //== Test == 判断如果场景线中怪物数量少于 2 创建怪物
            if (m_CurrPVPSceneLine.RoleList.Count < 2)
            {
                for (int i = m_CurrPVPSceneLine.RoleList.Count; i < 2; i++)
                {
                    MonsterClient monsterClient = new MonsterClient();
                    m_CurrPVPSceneLine.RoleList.AddLast(monsterClient);

                    //设置怪的位置 ID 等信息
                    //计算出怪应该出现在哪个场景区域

                    m_CurrPVPSceneLine.AOIAreaDic[1].RoleClientList.AddLast(monsterClient);
                    if (i == 0)
                    {
                        monsterClient.CurrFsmManager.ChangeState(RoleState.Idle);
                    }
                    else
                    {
                        monsterClient.CurrFsmManager.ChangeState(RoleState.Run);
                    }
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
