using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.Model.IHandler
{
    /// <summary>
    /// 角色客户端处理接口 
    /// </summary>
    public interface IRoleClientFsmHandler
    {
        void Init(RoleClientBase roleClientBase);

        void OnUpdate();

        void Idle_OnEnter();
        void Idle_OnUpdate();
        void Idle_OnLeave();

        void Run_OnEnter();
        void Run_OnUpdate();
        void Run_OnLeave();

        void Attack_OnEnter();
        void Attack_OnUpdate();
        void Attack_OnLeave();

        void Hurt_OnEnter();
        void Hurt_OnUpdate();
        void Hurt_OnLeave();

        void Die_OnEnter();
        void Die_OnUpdate();
        void Die_OnLeave();

        void Dispose();
    }
}
