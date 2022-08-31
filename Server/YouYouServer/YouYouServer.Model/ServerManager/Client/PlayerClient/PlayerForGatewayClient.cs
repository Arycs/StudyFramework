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
        /// 当前角色在哪个游戏服
        /// </summary>
        public int CurrInGameServerId = 0;

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
                GatewayServerManager.RemovePlayerClient(this);

                if (CurrInGameServerId > 0)
                {
                    //告诉网关 角色下线了
                    GWS2GS_Offline protoOffLineGs = new GWS2GS_Offline() {AccountId = AccountId};
                    m_CurrHandler.CarrySendToGameServer(CurrInGameServerId, protoOffLineGs.ProtoId,
                        ProtoCategory.GatewayServer2GameServer, protoOffLineGs.ToByteArray());
                }
                
                //通知中心服务器 角色下线了
                GWS2WS_Offline protoOffLineWs = new GWS2WS_Offline() {AccountId = AccountId};
                m_CurrHandler.CarrySendToWorldServer(protoOffLineWs.ProtoId, protoOffLineWs.ToByteArray());
                
                Dispose();
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