﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.RoleFsm.RoleFsmState
{
    public class RoleFsmHurt: RoleFsmStateBase
    {
        public RoleFsmHurt(RoleFsm roleFsm) : base(roleFsm) { }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrFsm.CurrRoleClient.currRoleClientFsmHandler.Hurt_OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            CurrFsm.CurrRoleClient.currRoleClientFsmHandler.Hurt_OnLeave();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrFsm.CurrRoleClient.currRoleClientFsmHandler.Hurt_OnUpdate();
        }
    }
}
