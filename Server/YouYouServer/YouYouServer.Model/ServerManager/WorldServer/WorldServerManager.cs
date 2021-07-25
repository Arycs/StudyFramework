using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using YouYouServer.Common;
using YouYouServer.Core.Logger;

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
            m_GatewayServerClientDic = new Dictionary<int, GatewayServerClient>();

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

                //实例化一个Socket Client
                new ServerClient(socket);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameServerClient"></param>
        public static void RegisterGameServerClient(GameServerClient gameServerClient){
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RegGameServer Success ServerId={1}", gameServerClient.ServerId);
            m_GameServerClientDic.Add(gameServerClient.ServerId, gameServerClient);
        }

        /// <summary>
        /// 注册网关服务器客户端
        /// </summary>
        /// <param name="gatewayServerClient"></param>
        public static void RegisterGatewayServerClient(GatewayServerClient gatewayServerClient)
        {
            LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "RegGatewayServer Success ServerId={1}", gatewayServerClient);
            m_GatewayServerClientDic.Add(gatewayServerClient.ServerId, gatewayServerClient);

            CheckAllServerClientRegisterComplete();
        }

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

            }
        }
    }
}
