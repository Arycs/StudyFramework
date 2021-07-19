using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager.Client;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 中心服务器管理器
    /// </summary>
    public sealed class WorldServerManager
    {
        /// <summary>
        /// 游戏服务器客户端字典
        /// </summary>
        private static Dictionary<int, GameServerClient> m_GameServerClientDic;

        /// <summary>
        /// 网关服务器客户端
        /// </summary>
        private static Dictionary<int, GatewayServerClient> m_GatewayServerClientDic;

        public static void Init()
        {
            m_GameServerClientDic = new Dictionary<int, GameServerClient>();
            m_GatewayServerClientDic = new Dictionary<int, GatewayServerClient>();
        }
    }
}
