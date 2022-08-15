﻿using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using YouYouServer.Common;
using YouYouServer.Core;

namespace YouYouServer.Model
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
        private static Dictionary<int, GatewayServerForWorldClient> m_GatewayServerClientDic;

        /// <summary>
        /// 中心服务器上的玩家字典
        /// </summary>
        private static Dictionary<long, PlayerForWorldClient> m_PlayerForWorldClientDic;

        /// <summary>
        /// 当前服务器
        /// </summary>
        public static ServerConfig.Server CurrServer;

        /// <summary>
        /// 游戏服配置列表
        /// </summary>
        private static List<ServerConfig.Server> LstGameServer = null;

        /// <summary>
        /// 网关服配置列表
        /// </summary>
        private static List<ServerConfig.Server> LstGatewayServer = null;

        /// <summary>
        /// Socket监听
        /// </summary>
        private static Socket m_ListenSocket;

        public static void Init()
        {
            m_GameServerClientDic = new Dictionary<int, GameServerClient>();
            m_GatewayServerClientDic = new Dictionary<int, GatewayServerForWorldClient>();
            m_PlayerForWorldClientDic = new Dictionary<long, PlayerForWorldClient>();

            CurrServer = ServerConfig.GetCurrServer();
            LstGameServer = ServerConfig.GetServerByType(ConstDefine.ServerType.GameServer);
            LstGatewayServer = ServerConfig.GetServerByType(ConstDefine.ServerType.GatewayServer);

            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "CurrServer={0}", CurrServer.ServerId);

            StarListen();
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        private static void StarListen()
        {
            //实例化Socket
            m_ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //向操作系统申请一个可用的ip和端口用来通讯
            m_ListenSocket.Bind(new IPEndPoint(IPAddress.Parse(CurrServer.Ip), CurrServer.Port));

            m_ListenSocket.Listen(20);

            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "启动监听{0}成功", m_ListenSocket.LocalEndPoint.ToString());

            Thread mThread = new Thread(ListenClientCallBack);
            mThread.Start();
        }

        /// <summary>
        /// 监听回调
        /// </summary>
        /// <param name="obj"></param>
        private static void ListenClientCallBack(object obj)
        {
            while (true)
            {
                //接受客户端请求
                Socket socket = m_ListenSocket.Accept();

                IPEndPoint point = (IPEndPoint)socket.RemoteEndPoint;

                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "客户端IP={0} Port={1} 已经连接", point.Address.ToString(), point.Port.ToString());

                //实例化一个Socket Client
                new ServerClient(socket);
            }
        }

        /// <summary>
        /// 注册游戏服务器客户端
        /// </summary>
        /// <param name="gameServerClient"></param>
        public static void RegisterGameServerClient(GameServerClient gameServerClient) {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RegGameServer Success ServerId={0}", gameServerClient.ServerId);
            m_GameServerClientDic.Add(gameServerClient.ServerId, gameServerClient);

            CheckAllServerClientRegisterComplete();
        }
        
        /// <summary>
        /// 移除游戏服务器客户端
        /// </summary>
        /// <param name="gameServerClient"></param>
        public static void RemoveGameServerClient(GameServerClient gameServerClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RemoveGameServerClient Success ServerId = {0}", gameServerClient.ServerId);
            m_GameServerClientDic.Remove(gameServerClient.ServerId);
        }


        /// <summary>
        /// 注册网关服务器客户端
        /// </summary>
        /// <param name="gatewayServerClient"></param>
        public static void RegisterGatewayServerClient(GatewayServerForWorldClient gatewayServerForWorldClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RegGatewayServer Success ServerId={1}", gatewayServerForWorldClient.ServerId);
            m_GatewayServerClientDic.Add(gatewayServerForWorldClient.ServerId, gatewayServerForWorldClient);

            CheckAllServerClientRegisterComplete();
        }

        #region RemoveGatewayServerClient 移除网关服务器客户端
        public static void RemoveGatewayServerClient(GatewayServerForWorldClient gatewayServerForWorldClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RemoveGatewayServerClient Success ServerId = {0}", gatewayServerForWorldClient.ServerId);
            m_GatewayServerClientDic.Remove(gatewayServerForWorldClient.ServerId);
        }
        #endregion

        #region CheckAllServerClientRegisterComplete 检查所有服务器客户端注册完毕
        /// <summary>
        /// 检查所有服务器客户端注册完毕
        /// </summary>
        private static void CheckAllServerClientRegisterComplete()
        {
            if (LstGameServer == null || LstGatewayServer == null)
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "CheckAllServerClientRegisterComplete Fail No ServerConfig");
                return;
            }
            if (LstGameServer.Count == m_GameServerClientDic.Count && LstGatewayServer.Count == m_GatewayServerClientDic.Count)
            {
                //所有服务器都注册完毕
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "AllServerClientRegisterComplete");
                //中心服务器通知所有网关服务器 可以注册到游戏服 
                var enumerator = m_GatewayServerClientDic.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    GatewayServerForWorldClient gatewayServerClient = enumerator.Current.Value;
                    gatewayServerClient.SendToRegGameServer();
                }
            }
        }
        #endregion

        #region CheckAllGatewayServerRegisterGameServerComplete 检查所有网关服务器注册到游戏服务器完毕
        /// <summary>
        /// 检查所有网关服务器注册到游戏服务器完毕
        /// </summary>
        public static void CheckAllGatewayServerRegisterGameServerComplete()
        {
            bool regGameServerComplete = true;
            var enumerator = m_GatewayServerClientDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                GatewayServerForWorldClient gatewayServerClient = enumerator.Current.Value;
                if (gatewayServerClient.CurrServerStatus != ConstDefine.GatewayServerStatus.RegGameServerSuccess)
                {
                    regGameServerComplete = false;
                    break;
                }
            }
            ///如果所有网关服务器注册到游戏服务器完毕
            if (regGameServerComplete)
            {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "AllGatewayServerRegisterGameServerComplete");
                //Todo 通知web服务器
            }
        }
        #endregion

        #region RegisterPlayerForWorldClient 注册中心服务器上的玩家客户端
        /// <summary>
        /// 注册中心服务器上的玩家客户端
        /// </summary>
        /// <param name="playerForWorldClient"></param>
        public static void RegisterPlayerForWorldClient(PlayerForWorldClient playerForWorldClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RegisterPlayerForwaorldClient Success ServerId={1}", playerForWorldClient.AccountId);
            m_PlayerForWorldClientDic.Add(playerForWorldClient.AccountId, playerForWorldClient);

        }
        #endregion

        #region GetPlayerClient 获取注册中心服务器上的玩家客户端
        /// <summary>
        /// 获取注册中心服务器上的玩家客户端
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static PlayerForWorldClient GetPlayerClient(long accountId)
        {
            PlayerForWorldClient playerForWorldClient = null;
            m_PlayerForWorldClientDic.TryGetValue(accountId, out playerForWorldClient);
            return playerForWorldClient;
        }
        #endregion

        #region RemoveGameServerClient 移除中心服务器上的玩家客户端
        public static void RemovePlayerForWorldClient(PlayerForWorldClient playerForWorldClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RemovePlayerForwaorldClient Success ServerId={1}", playerForWorldClient.AccountId);
            m_PlayerForWorldClientDic.Remove(playerForWorldClient.AccountId);

        }
        #endregion

        
    }
}
