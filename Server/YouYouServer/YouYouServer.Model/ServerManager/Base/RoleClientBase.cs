using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.IHandler;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 角色客户端基类
    /// </summary>
    public abstract class RoleClientBase
    {
        public RoleClientBase() { }

        /// <summary>
        /// 当前角色客户端处理句柄
        /// </summary>
        public IRoleClientHandler CurrRoleClientHandler;

        /// <summary>
        /// 当前的状态机管理器
        /// </summary>
        public RoleFsm.RoleFsm CurrFsmManager;

        public void OnUpdate()
        {
            CurrRoleClientHandler?.OnUpdate();
            CurrFsmManager?.OnUpdate();
        }
    }
}
