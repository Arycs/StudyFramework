using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core.Common;
using YouYouServer.Core.Logger;
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

        /// <summary>
        /// 当前服务器
        /// </summary>
        public static ServerConfig.Server CurrServer;

        /// <summary>
        /// 中心服务器
        /// </summary>
        public static ServerConfig.Server WorldServer;

        /// <summary>
        /// Socket连接器
        /// </summary>
        private static Socket m_ListenSocket;

        /// <summary>
        /// Socket连接器
        /// </summary>
        private static ClientSocket m_ConnectToWorldSocket;

        /// <summary>
        /// Socket事件监听派发器
        /// </summary>
        private static EventDispatcher m_EventDispatcher;

        public static void Init()
        {
            m_GatewayServerClientDic = new Dictionary<int, GatewayServerClient>();
            m_EventDispatcher = new EventDispatcher();

            CurrServer = ServerConfig.GetCurrServer();

            List<ServerConfig.Server> servers = ServerConfig.GetServerByType(ConstDefine.ServerType.WorldServer);
            if (servers != null && servers.Count == 1)
            {
                WorldServer = servers[0];

                ConnectToWorldServer();
            }
            else
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError,LogType.SysLog,"No WorldServer");
            }
        }

        /// <summary>
        /// 连接到中心服务器
        /// </summary>
        private static void ConnectToWorldServer()
        {
            m_ConnectToWorldSocket = new ClientSocket(m_EventDispatcher);
            m_ConnectToWorldSocket.OnConnectSuccess = () => {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "ConnectToWorldServer Success");
            };

            m_ConnectToWorldSocket.OnConnectFail = () => {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "ConnectToWorldServer Fail");
            };
            m_ConnectToWorldSocket.Connect(WorldServer.Ip, WorldServer.Port);
        }
    }
}
