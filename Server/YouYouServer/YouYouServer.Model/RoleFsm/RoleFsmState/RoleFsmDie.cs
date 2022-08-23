using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.RoleFsm.RoleFsmState
{
    public class RoleFsmDie : RoleFsmStateBase
    {
        public RoleFsmDie(RoleFsm roleFsm) : base(roleFsm) { }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Die_OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Die_OnLeave();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Die_OnUpdate();
        }
    }
}
