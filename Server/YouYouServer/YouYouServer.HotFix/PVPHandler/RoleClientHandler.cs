using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.HotFix.PVPHandler
{
    [Handler(ConstDefine.RoleClientHandler)]
    public class RoleClientHandler :IRoleClientHandler
    {
        public void Init(RoleClientBase roleClientBase) { }

        public void Attack_OnEnter()
        {
        }

        public void Attack_OnLeave()
        {
        }

        public void Attack_OnUpdate()
        {
        }

        public void Die_OnEnter()
        {
        }

        public void Die_OnLeave()
        {
        }

        public void Die_OnUpdate()
        {
        }

        public void Dispose()
        {
        }

        public void Hurt_OnEnter()
        {
        }

        public void Hurt_OnLeave()
        {
        }

        public void Hurt_OnUpdate()
        {
        }

        public void Idle_OnEnter()
        {
            Console.WriteLine("Idle_OnEnter");
        }

        public void Idle_OnLeave()
        {
        }

        public void Idle_OnUpdate()
        {
        }

        public void OnUpdate()
        {
        }

        public void Run_OnEnter()
        {
            Console.WriteLine("Run_OnEnter");
        }

        public void Run_OnLeave()
        {
        }

        public void Run_OnUpdate()
        {
        }
    }
}
