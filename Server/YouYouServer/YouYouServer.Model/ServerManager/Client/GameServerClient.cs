using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 游戏服务器客户端
    /// </summary>
    public class GameServerClient : ServerClientBase
    {
        public GameServerClient(ServerClient serverClient) : base(serverClient)
        {
            //断开连接时
            CurrServerClient.OnDisConnect = () =>
            {
                Dispose();
                WorldServerManager.RemoveGameServerClient(this);
            };
            AddEventListener();
        }

        /// <summary>
        /// 监听游戏服务器发来的消息
        /// </summary>
        public override void AddEventListener()
        {
            base.AddEventListener();
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
        }

    }
}
