using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core.Common;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 服务器客户端
    /// </summary>
    public class ServerClient : IDisposable
    {
        /// <summary>
        /// 服务器编号
        /// </summary>
        public int ServerId
        {
            get; private set;
        }

        /// <summary>
        /// Socket时间监听派发器
        /// </summary>
        public EventDispatcher EventDispatcher
        {
            get; private set;
        }

        /// <summary>
        /// Socket连接器
        /// </summary>
        private ClientSocket m_ClientSocket;

        /// <summary>
        /// 发送协议时的MS缓存
        /// </summary>
        public MMO_MemoryStream SendProtoMS
        {
            get; private set;
        }

        /// <summary>
        /// 解析协议时的MS缓存
        /// </summary>
        public MMO_MemoryStream GetProtoMS
        {
            get; private set;
        }

        public ServerClient(Socket socket)
        {
            EventDispatcher = new EventDispatcher();
            SendProtoMS = new MMO_MemoryStream();
            GetProtoMS = new MMO_MemoryStream();

            m_ClientSocket = new ClientSocket(socket, EventDispatcher);
            AddEventListener();
        }

        /// <summary>
        /// 监听服务器客户端 连接到服务器
        /// </summary>
        private void AddEventListener()
        {
            EventDispatcher.AddEventListener(ProtoCodeDef.GameServer2CenterServer_RegGameServer, OnGameServer2CenterServer_RegGameServer);
            EventDispatcher.AddEventListener(ProtoCodeDef.GatewayServer2CenterServer_RegGatewayServer, OnGatewayServer2CenterServer_RegGatewayServer);
        }

        private void RemoveEventListener()
        {
            EventDispatcher.RemoveEventListener(ProtoCodeDef.GameServer2CenterServer_RegGameServer, OnGameServer2CenterServer_RegGameServer);
        }

        /// <summary>
        /// 游戏服务器注册到中心服务器
        /// </summary>
        /// <param name="buffer"></param>
        private void OnGameServer2CenterServer_RegGameServer(byte[] buffer)
        {
            GameServer2CenterServer_RegGameServerProto proto = GameServer2CenterServer_RegGameServerProto.GetProto(GetProtoMS, buffer);
            ServerConfig.Server server = ServerConfig.GetServer(ConstDefine.ServerType.GameServer, proto.ServerId);
            if (server != null)
            {
                ServerId = proto.ServerId;

                WorldServerManager.RegisterGameServerClient(new GameServerClient(this));
            }
            else
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "RegGameServer Fail ServerId = {0}", proto.ServerId);
            }

        }

        /// <summary>
        /// 网关服务器注册到中心服务器
        /// </summary>
        /// <param name="buffer"></param>
        private void OnGatewayServer2CenterServer_RegGatewayServer(byte[] buffer)
        {
            GatewayServer2CenterServer_RegGatewayServerProto proto = GatewayServer2CenterServer_RegGatewayServerProto.GetProto(GetProtoMS, buffer);
            ServerConfig.Server server = ServerConfig.GetServer(ConstDefine.ServerType.GatewayServer, proto.ServerId);
            if (server != null)
            {
                ServerId = proto.ServerId;

                WorldServerManager.RegisterGatewayServerClient(new GatewayServerClient(this));
            }
            else
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "RegGatewayServer Fail ServerId = {0}", proto.ServerId);
            }
        }

        public void Dispose()
        {
            RemoveEventListener();
        }
    }
}
