using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.ServerManager
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

        /// <summary>
        /// 发送协议时候的MS缓存
        /// </summary>
        public MMO_MemoryStream SendProtoMS
        {
            get; private set;
        }

        /// <summary>
        /// 解析协议时候的MS缓存
        /// </summary>
        public MMO_MemoryStream GetProtoMS
        {
            get;private set;
        }

        public PlayerClientBase()
        {
            EventDispatcher = new EventDispatcher();
            SendProtoMS = new MMO_MemoryStream();
            GetProtoMS = new MMO_MemoryStream();
        }

        /// <summary>
        /// 监听玩家客户端的消息
        /// </summary>
        public virtual void AddEventListener()
        {

        }

        /// <summary>
        /// 移除监听玩家客户端的消息
        /// </summary>
        public virtual void RemoveEventListener()
        {

        }

        public void Dispose()
        {
            RemoveEventListener();
        }
    }
}
