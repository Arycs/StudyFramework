using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
        private static Dictionary<long, PlayerForGatewayClient> m_PlayerClientDic;

        /// <summary>
        /// 当前服务器
        /// </summary>
        public static ServerConfig.Server CurrServer;

        /// <summary>
        /// 游戏服配置列表
        /// </summary>
        private static List<ServerConfig.Server> LstGameServer = null;

        /// <summary>
        /// 连接到中心服务器代理
        /// </summary>
        public static GatewayConnectWorldAgent ConnectWorldAgent;

        /// <summary>
        /// Socket监听
        /// </summary>
        private static Socket m_ListenSocket;

        

        public static void Init()
        {
            m_PlayerClientDic = new Dictionary<long, PlayerForGatewayClient>();

            CurrServer = ServerConfig.GetCurrServer();
            LstGameServer = ServerConfig.GetServerByType(ConstDefine.ServerType.GameServer);

            //实例化连接到中心服务器代理
            ConnectWorldAgent = new GatewayConnectWorldAgent();
            ConnectWorldAgent.RegisterToWorldServer();

            StarListen();
        }

        #region ToRegGameServer 注册到游戏服
        /// <summary>
        /// 注册到游戏服
        /// </summary>
        public static void ToRegGameServer()
        {
            //拿到要目标游戏服列表
            int len = LstGameServer.Count;
            for (int i = 0; i < len; i++)
            {
                GatewayConnectGameAgent agent = new GatewayConnectGameAgent(LstGameServer[i]);
                agent.RegisterToGameServer();
            }

            //通知中心服务器 注册游戏服完毕
            ConnectWorldAgent.ToRegGameServerSuccess();
        }
        #endregion


        #region StarListen 启动监听
        /// <summary>
        /// 启动监听
        /// </summary>
        private static void StarListen()
        {
            //实例化Socket
            m_ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //向操作系统申请一个可用的IP 和 端口来通讯
            m_ListenSocket.Bind(new IPEndPoint(IPAddress.Parse(CurrServer.Ip), CurrServer.Port));

            m_ListenSocket.Listen(3000);

            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "启动监听 {0} 成功", m_ListenSocket.LocalEndPoint.ToString());

            Thread mThread = new Thread(ListenClientCallBack);
            mThread.Start();
        }

        
        #endregion

        #region ListenClientCallBack 监听回调
        private static void ListenClientCallBack(object obj)
        {
            while (true)
            {
                //接受服务器客户端请求
                Socket socket = m_ListenSocket.Accept();

                IPEndPoint point = (IPEndPoint)socket.RemoteEndPoint;

                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "客户端IP = {0} Port={1} 已经连接", point.Address.ToString(), point.Port.ToString());

                //实例化一个服务器客户端
                new PlayerForGatewayClient(socket);
            }
        }
        #endregion

        /// <summary>
        /// 注册玩家客户端
        /// </summary>
        /// <param name="playerClient"></param>
        public static void RegisterPlayerClient(PlayerForGatewayClient playerClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RegisterPlayerClient Success AccountId = {0}", playerClient.AccountId);
            m_PlayerClientDic.Add(playerClient.AccountId, playerClient);
        }

        /// <summary>
        /// 移除玩家客户端
        /// </summary>
        /// <param name="playerForGatewayClient"></param>
        public static void RemovePlayerClient(PlayerForGatewayClient playerForGatewayClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RemovePlayerClient Success AccountId = {0}", playerForGatewayClient.AccountId);
            m_PlayerClientDic.Remove(playerForGatewayClient.AccountId);
        }

        /// <summary>
        /// 通过ID 获取玩家客户端
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static PlayerForGatewayClient GetPlayerClient(long accountId)
        {
            PlayerForGatewayClient playerForGatewayClient = null;
            m_PlayerClientDic.TryGetValue(accountId, out playerForGatewayClient);
            return playerForGatewayClient;
        }
    }
}
