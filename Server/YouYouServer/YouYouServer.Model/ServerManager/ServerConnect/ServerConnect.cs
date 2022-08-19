using System;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Core.Common;

namespace YouYouServer.Model
{
    /// <summary>
    /// 中心服务器连接器
    /// </summary>
    public class ServerConnect
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
        /// 当前的服务器配置
        /// </summary>
        private ServerConfig.Server m_CurrConfig;

        /// <summary>
        /// 处理中转协议
        /// </summary>
        public BaseAction<ushort, ProtoCategory, byte[]> OnCarryProto;

        public ServerConnect(ServerConfig.Server serverConfig)
        {
            m_CurrConfig = serverConfig;

            EventDispatcher = new EventDispatcher();
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

            //处理服务器返回的中转协议
            ClientSocket.OnCarryProto = (ushort protoCode, ProtoCategory protoCategory, byte[] buffer) => {
                OnCarryProto?.Invoke(protoCode, protoCategory, buffer);
            };
            ClientSocket.Connect(m_CurrConfig.Ip, m_CurrConfig.Port);
        }
    }
}
