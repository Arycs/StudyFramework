using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.Common;
using YouYouServer.Core.Logger;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 中心服务器连接器
    /// </summary>
    public class WorldServerConnect
    {
        /// <summary>
        /// Socket 事件监听派发器
        /// </summary>
        public EventDispatcher EventDispatcher
        {
            get; private set;
        }

        /// <summary>
        /// Socket连接器
        /// </summary>
        public ClientSocket ClientSocket
        {
            get; private set;
        }

        /// <summary>
        /// 发送协议时候的MS缓存
        /// </summary>
        public MMO_MemoryStream SendProtoMS
        {
            get; private set;
        }


        /// <summary>
        /// 解析协议时候的MS缓存
        /// </summary>
        public MMO_MemoryStream GetProtoMS
        {
            get; private set;
        }


        /// <summary>
        /// 当前的服务器配置
        /// </summary>
        private ServerConfig.Server m_CurrConfig;

        public WorldServerConnect(ServerConfig.Server serverConfig)
        {
            m_CurrConfig = serverConfig;

            EventDispatcher = new EventDispatcher();
            SendProtoMS = new MMO_MemoryStream();
            GetProtoMS = new MMO_MemoryStream();
        }

        /// <summary>
        /// 连接到中心服务器
        /// </summary>
        /// <param name="onConnectSuccess"></param>
        /// <param name="onConnectFail"></param>
        public void Connect(Action onConnectSuccess = null, Action onConnectFail = null)
        {
            ClientSocket = new ClientSocket(EventDispatcher);
            ClientSocket.OnConnectSuccess = () =>
            {
                LoggerMgr.Log(Core.LoggerLevel.Log, LogType.SysLog, "Connect WorldServer Success");
                onConnectSuccess?.Invoke();
            };

            ClientSocket.OnConnectFail = () =>
            {
                LoggerMgr.Log(Core.LoggerLevel.LogError, LogType.SysLog, "Connect WorldServer Fail");
                onConnectFail?.Invoke();
            };
            ClientSocket.Connect(m_CurrConfig.Ip, m_CurrConfig.Port);
        }
    }


}
