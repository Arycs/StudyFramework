using System;
using System.Collections.Generic;
using System.Text;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 游戏服务器客户端
    /// </summary>
    public class GameServerClient : IDisposable
    {
        /// <summary>
        /// 当前服务器客户端
        /// </summary>
        private ServerClient m_CurrServerClient;

        /// <summary>
        /// 服务器编号
        /// </summary>
        public int ServerId
        {
            get; private set;
        }

        public GameServerClient(ServerClient serverClient)
        {
            m_CurrServerClient = serverClient;
            ServerId = m_CurrServerClient.ServerId;

            AddEventListener();
        }

        /// <summary>
        /// 监听游戏服务器发来的消息
        /// </summary>
        private void AddEventListener()
        {

        }

        /// <summary>
        /// 移除监听
        /// </summary>
        private void RemoveEventListener()
        {

        }

        public void Dispose()
        {

        }
    }
}
