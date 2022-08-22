using System;
using System.Collections.Generic;
using System.Text;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.Model
{
    /// <summary>
    /// 游戏服务器上的玩家客户端
    /// </summary>
    public class PlayerClientBase : RoleClientBase,IDisposable
    {
        /// <summary>
        /// 玩家账号
        /// </summary>
        public long AccountId;

        /// <summary>
        /// Socket 事件监听派发器
        /// </summary>
        public EventDispatcher EventDispatcher
        {
            get; private set;
        }

        public PlayerClientBase()
        {
            EventDispatcher = new EventDispatcher();
            CurrFsmManager = new RoleFsm.RoleFsm(this);
        }

        public void Dispose()
        {

        }

        public override RoleType CurrRoleType => RoleType.Player;
        
        /// <summary>
        /// 当前角色
        /// </summary>
        public RoleEntity CurrRole { get; set; }
    }
}
