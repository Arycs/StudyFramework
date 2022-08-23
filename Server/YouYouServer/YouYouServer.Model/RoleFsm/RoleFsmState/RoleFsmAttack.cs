using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.RoleFsm.RoleFsmState
{
    public class RoleFsmAttack : RoleFsmStateBase
    {
        public RoleFsmAttack (RoleFsm roleFsm) : base(roleFsm) { }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Attack_OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Attack_OnLeave();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Attack_OnUpdate();
        }
    }
}
