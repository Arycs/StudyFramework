using System;
using System.Collections.Generic;
using System.Text;
using YouYou.Proto;
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
        /// 角色编号
        /// </summary>
        public long RoleId;

        /// <summary>
        /// 当前所在区域编号
        /// </summary>
        public int CurrAreaId;
        
        /// <summary>
        /// 当前角色客户端处理句柄
        /// </summary>
        public IRoleClientHandler CurrRoleClientHandler;

        /// <summary>
        /// 当前的状态机管理器
        /// </summary>
        public RoleFsm.RoleFsm CurrFsmManager;

        /// <summary>
        /// 当前角色类型
        /// </summary>
        public abstract RoleType CurrRoleType { get; }

        public void OnUpdate()
        {
            CurrRoleClientHandler?.OnUpdate();
            CurrFsmManager?.OnUpdate();
        }
    }
}
