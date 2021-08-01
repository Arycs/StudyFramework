using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Common;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 游戏服务器连接到中心服务器代理
    /// </summary>
    public class GameConnectWorldAgent : ConnectAgentBase
    {
        public GameConnectWorldAgent()
        {
            List<ServerConfig.Server> servers = ServerConfig.GetServerByType(ConstDefine.ServerType.WorldServer);
            if (servers != null && servers.Count == 1)
            {
                TargetServerConfig = servers[0];

                //连接到中心服务器
                TargetServerConnect = new ServerConnect(TargetServerConfig);
                AddEventListener();
            }
            else
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "No WorldServer");
            }
        }

        /// <summary>
        /// 监听中心服务器发来的消息
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

        #region RegisterToWorldServer 注册到中心服务器
        /// <summary>
        /// 注册到中心服务器
        /// </summary>
        public void RegisterToWorldServer()
        {
            TargetServerConnect.Connect(onConnectSuccess: (Action)(() =>
            {
                //告诉中心服务器 我是谁
                GS2WS_RegGameServerProto proto = new GS2WS_RegGameServerProto();
                proto.ServerId = GameServerManager.CurrServer.ServerId;
                TargetServerConnect.ClientSocket.SendMsg(proto.ToArray((Core.Common.MMO_MemoryStream)TargetServerConnect.SendProtoMS));
            }));
        }
        #endregion
    }
}
