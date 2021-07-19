using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager.Client;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 游戏服客户端
    /// </summary>
    public sealed class GameServerManager
    {
        /// <summary>
        /// 网关服务器客户端
        /// </summary>
        private static Dictionary<int, GatewayServerClient> m_GatewayServerClientDic;

        public static void Init()
        {
            m_GatewayServerClientDic = new Dictionary<int, GatewayServerClient>();
        }
    }
}
