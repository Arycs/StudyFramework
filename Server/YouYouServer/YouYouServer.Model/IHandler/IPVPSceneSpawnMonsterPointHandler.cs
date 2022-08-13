using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.SceneManager.PVPScene;

namespace YouYouServer.Model.IHandler
{
    public interface IPVPSceneSpawnMonsterPointHandler
    {
        public void Init(PVPSceneSpawnMonsterPoint pvpSceneSpawnMonsterPoint);

        public void Dispose();

        public void OnUpdate();
    }
}
