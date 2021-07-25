using System.Collections.Generic;
using YouYouServer.Common;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 网关服务管理器
    /// </summary>
    public sealed class GatewayServerManager
    {
        /// <summary>
        /// 玩家客户端
        /// </summary>
        private static Dictionary<int, PlayerClient> m_PlayerClientDic;

        /// <summary>
        /// 当前服务器
        /// </summary>
        public static ServerConfig.Server CurrServer;

        /// <summary>
        /// 连接到中心服务器代理
        /// </summary>
        public static GatewayConnectWorldAgent ConnectWorldAgent;

        public static void Init()
        {
            m_PlayerClientDic = new Dictionary<int, PlayerClient>();

            CurrServer = ServerConfig.GetCurrServer();

            //实例化连接到中心服务器代理
            ConnectWorldAgent = new GatewayConnectWorldAgent();
            ConnectWorldAgent.RegisterToWorldServer();
        }

        
    }
}
