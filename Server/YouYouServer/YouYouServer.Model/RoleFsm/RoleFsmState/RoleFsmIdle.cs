using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.RoleFsm.RoleFsmState
{
    public class RoleFsmIdle: RoleFsmStateBase
    {
        public RoleFsmIdle(RoleFsm roleFsm) : base(roleFsm) { }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Idle_OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Idle_OnLeave();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Idle_OnUpdate();
        }
    }
}
