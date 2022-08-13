using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YouYouServer.Commmon;
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

        /// <summary>
        /// 上一次的执行时间
        /// </summary>
        private float m_PrevTime = 0;

        public void Init(PVPSceneLine pvpSceneLine)
        {
            m_CurrPVPSceneLine = pvpSceneLine;
            m_PrevTime = TimerManager.time;
        }

        public void SyncStatus()
        {
            //休眠 20 毫秒 1000/20 = 50帧 
            //TODO 帧率修改在这里 1000/X = Y帧
            Thread.Sleep(20);

            //设置到上一帧的时间
            m_CurrPVPSceneLine.Deltatime = TimerManager.time - m_PrevTime;
            m_PrevTime = TimerManager.time;

            //具体逻辑在这里写
            foreach (var item in m_CurrPVPSceneLine.AOIAreaDic)
            {
                item.Value.OnUpdate();
            }

            foreach (var item in m_CurrPVPSceneLine.SpawnMonsterPointDic)
            {
                item.Value.OnUpdate();
            }

        }

        public void Dispose()
        {
        }
    }
}
