using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core;

namespace YouYouServer.Model
{
    /// <summary>
    /// 游戏服务器上的玩家客户端
    /// </summary>
    public class PlayerClientBase : IDisposable
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
        }

        public void Dispose()
        {

        }
    }
}
