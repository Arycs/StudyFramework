using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using YouYouServer.Core;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 玩家客户端
    /// </summary>
    public class PlayerForGatewayClient : PlayerClientBase
    {
        /// <summary>
        /// 玩家在网关服务器的客户端
        /// </summary>
        public int CurrInGameServerId = 1;

        /// <summary>
        /// Socket连接器
        /// </summary>
        public ClientSocket ClientSocket
        {
            get; private set;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public Action OnDisConnect;

        public PlayerForGatewayClient(Socket socket) : base()
        {
            ClientSocket = new ClientSocket(socket, EventDispatcher);
            ClientSocket.OnDisConnect = () =>
            {
                Dispose();
                GatewayServerManager.RemovePlayerClient(this);

                //Todo 通知中心服务器和游戏服 玩家下线了 
            };
            //处理中转协议
            ClientSocket.OnCarryProto = OnCarryProto;

            AddEventListener();
        }

        /// <summary>
        /// 收到中转协议并处理
        /// </summary>
        /// <param name="protoCode">协议编号</param>
        /// <param name="protoCategory">协议分类</param>
        /// <param name="buffer">协议内容</param>
        private void OnCarryProto(ushort protoCode, ProtoCategory protoCategory, byte[] buffer)
        {
            switch (protoCategory)
            {
                case ProtoCategory.CarryProto:
                    break;
                case ProtoCategory.Client2GameServer:
                    break;
                case ProtoCategory.Client2WorldServer:
                    {
                        CarrySendToWorldServer(protoCode, buffer);
                    }
                    break;
            }
        }

        /// <summary>
        /// 中转发送到中心服的消息
        /// </summary>
        /// <param name="protoCode"></param>
        /// <param name="buffer"></param>
        private void CarrySendToWorldServer(ushort protoCode, byte[] buffer)
        {
            CarryProto proto = new CarryProto(AccountId, protoCode, ProtoCategory.Client2WorldServer, buffer);
            //通过中心服务器连接代理
            GatewayServerManager.ConnectWorldAgent.TargetServerConnect.ClientSocket.SendMsg(proto.ToArray(GatewayServerManager.ConnectWorldAgent.TargetServerConnect.SendProtoMS));
        }

        /// <summary>
        /// 监听玩家客户端的消息
        /// </summary>
        public override void AddEventListener()
        {
            base.AddEventListener();
            EventDispatcher.AddEventListener(ProtoCodeDef.C2GWS_RegClient, OnC2GWS_RegClient);
        }

        /// <summary>
        /// 移除玩家客户端的消息
        /// </summary>
        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            EventDispatcher.RemoveEventListener(ProtoCodeDef.C2GWS_RegClient, OnC2GWS_RegClient);
        }

        /// <summary>
        /// 注册客户端
        /// </summary>
        /// <param name="buffer"></param>
        private void OnC2GWS_RegClient(byte[] buffer)
        {
            C2GWS_RegClientProto proto = C2GWS_RegClientProto.GetProto(GetProtoMS, buffer);

            //此处可以加个验证  验证账号合法性

            AccountId = proto.AccountId;
            GatewayServerManager.RegisterPlayerClient(this);
            SendRegClientResult();
        }

        /// <summary>
        /// 向客户端发送注册结果
        /// </summary>
        private void SendRegClientResult()
        {
            GWS2C_ReturnRegClientProto proto = new GWS2C_ReturnRegClientProto();
            proto.Result = true;
            ClientSocket.SendMsg(proto.ToArray(SendProtoMS));
        }
    }
}
