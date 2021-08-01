using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 网关服务器链接到游戏服务器代理
    /// </summary>
    public class GatewayConnectGameAgent : ConnectAgentBase
    {
        public GatewayConnectGameAgent(ServerConfig.Server server)
        {
            TargetServerConfig = server;
            if (TargetServerConfig != null)
            {
                //连接到游戏服务器
                TargetServerConnect = new ServerConnect(TargetServerConfig);
                AddEventListener();
            }
            else
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "No GameServer");
            }
        }
        /// <summary>
        /// 添加监听
        /// </summary>
        public override void AddEventListener()
        {
            base.AddEventListener();
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
        }

        #region RegisterToGameServer 注册到游戏服务器
        /// <summary>
        /// 注册到游戏服务器
        /// </summary>
        public void RegisterToGameServer()
        {
            TargetServerConnect.Connect(onConnectSuccess: (Action)(() =>
            {
                //告诉游戏服务器 我是谁
                GWS2GS_RegGatewayServerProto proto = new GWS2GS_RegGatewayServerProto();
                proto.ServerId = GatewayServerManager.CurrServer.ServerId;
                TargetServerConnect.ClientSocket.SendMsg(proto.ToArray((Core.Common.MMO_MemoryStream)TargetServerConnect.SendProtoMS));
            }));
        }
        #endregion
    }
}
