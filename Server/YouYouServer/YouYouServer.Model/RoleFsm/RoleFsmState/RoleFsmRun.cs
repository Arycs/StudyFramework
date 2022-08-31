﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.RoleFsm.RoleFsmState
{
    public class RoleFsmRun : RoleFsmStateBase
    {
        public RoleFsmRun(RoleFsm roleFsm) : base(roleFsm) { }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void OnReSet()
        {
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Run_OnReSet();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Run_OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Run_OnLeave();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            CurrFsm.CurrRoleClient.CurrRoleClientFsmHandler.Run_OnUpdate();
        }
    }
}
