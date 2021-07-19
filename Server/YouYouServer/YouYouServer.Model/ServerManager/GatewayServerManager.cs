using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.ServerManager.Client;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 网关服客户端
    /// </summary>
    public sealed class GatewayServerManager
    {
        /// <summary>
        /// 玩家客户端
        /// </summary>
        private static Dictionary<int, PlayerClient> m_PlayerClientDic;

        public static void Init()
        {
            m_PlayerClientDic = new Dictionary<int, PlayerClient>();
        }
    }
}
