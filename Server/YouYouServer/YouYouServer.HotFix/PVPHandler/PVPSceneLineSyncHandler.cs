using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;

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
            Console.WriteLine("服务器时刻={0}",DateTime.UtcNow.Ticks);
            //具体逻辑在这里写
        }

        public void Dispose()
        {
        }
    }
}
