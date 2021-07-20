using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using YouYouServer.Core.Logger;
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

        /// <summary>
        /// 当前服务器
        /// </summary>
        public static ServerConfig.Server CurrServer;

        /// <summary>
        /// Socket监听
        /// </summary>
        private static Socket m_ListenSocket;

        public static void Init()
        {
            m_GameServerClientDic = new Dictionary<int, GameServerClient>();
            m_GatewayServerClientDic = new Dictionary<int, GatewayServerClient>();

            CurrServer = ServerConfig.GetCurrServer();
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

            //想操作系统申请一个可用的ip和端口用来通讯
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
                  
            }
        }
    }
}
