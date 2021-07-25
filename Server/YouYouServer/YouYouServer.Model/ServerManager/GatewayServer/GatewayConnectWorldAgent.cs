using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 网关服务器链接中心服务器代理
    /// </summary>
    public class GatewayConnectWorldAgent : IDisposable
    {
        /// <summary>
        /// 中心服务器配置
        /// </summary>
        public ServerConfig.Server WorldServerConfig;

        /// <summary>
        /// 当前中心服务器连接器
        /// </summary>
        public WorldServerConnect CurrWorldServerConnect;

        public GatewayConnectWorldAgent()
        {
            AddEventListener();
        }

        public void Dispose()
        {
            RemoveEventListener();
        }

        /// <summary>
        /// 监听中心服务器发来的消息
        /// </summary>
        private void AddEventListener()
        {

        }

        /// <summary>
        /// 移除监听
        /// </summary>
        private void RemoveEventListener()
        {

        }

        #region   RegisterToWorldServer 注册到中心服务器  
        public void RegisterToWorldServer()
        {
            List<ServerConfig.Server> servers = ServerConfig.GetServerByType(ConstDefine.ServerType.WorldServer);
            if (servers != null && servers.Count == 1)
            {
                WorldServerConfig = servers[0];

                //连接到中心服务器
                CurrWorldServerConnect = new WorldServerConnect(WorldServerConfig);
                CurrWorldServerConnect.Connect(onConnectSuccess: () =>
                {
                    //告诉中心服务器 我是谁
                    GatewayServer2CenterServer_RegGatewayServerProto proto = new GatewayServer2CenterServer_RegGatewayServerProto();
                    proto.ServerId = GatewayServerManager.CurrServer.ServerId;
                    CurrWorldServerConnect.ClientSocket.SendMsg(proto.ToArray(CurrWorldServerConnect.SendProtoMS));
                });
            }
            else
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "No WorldServer");
            }
        }
        #endregion
    }

}
