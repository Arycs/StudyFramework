using System;
using System.Net.Sockets;
using Google.Protobuf;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Core.Common;
using YouYouServer.Model.IHandler;

namespace YouYouServer.Model
{
    /// <summary>
    /// 玩家在网关服务器的客户端
    /// </summary>
    public class PlayerForGatewayClient : PlayerClientBase
    {
        /// <summary>
        /// 当前角色编号
        /// </summary>
        public long RoleId = 0;

        /// <summary>
        /// 当前角色在哪个游戏服
        /// </summary>
        public int CurrInGameServerId = 0;

        /// <summary>
        /// 当前角色在哪个场景
        /// </summary>
        public int CurrSceneId;

        /// <summary>
        /// PingValue
        /// </summary>
        public int PingValue;

        /// <summary>
        /// Socket连接器
        /// </summary>
        public ClientSocket ClientSocket { get; private set; }

        public PlayerForGatewayClient(Socket socket) : base()
        {
            ClientSocket = new ClientSocket(socket, EventDispatcher);
            ClientSocket.OnDisConnect = () =>
            {
                Dispose();
                GatewayServerManager.RemovePlayerClient(this);

                //TODO 通知中心服务器和 游戏服 玩家下线了
            };

            HotFixHelper.OnLoadAssembly += InitPlayerForGatewayClientHandler;
            InitPlayerForGatewayClientHandler();
        }

        private IPlayerForGatewayClientHandler m_CurrHandler;

        private void InitPlayerForGatewayClientHandler()
        {
            if (m_CurrHandler != null)
            {
                //把旧的实例释放
                m_CurrHandler.Dispose();
                m_CurrHandler = null;
            }

            m_CurrHandler =
                Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.PlayerForGatewayClientHandler]) as
                    IPlayerForGatewayClientHandler;
            m_CurrHandler?.Init(this);

            Console.WriteLine("InitPlayerForGatewayClientHandler");
        }
    }
}