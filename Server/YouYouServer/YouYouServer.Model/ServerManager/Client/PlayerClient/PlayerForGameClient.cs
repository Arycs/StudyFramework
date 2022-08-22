using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YouYou;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model.IHandler;

namespace YouYouServer.Model
{
    /// <summary>
    /// 游戏服务器上的玩家客户端
    /// </summary>
    public class PlayerForGameClient : PlayerClientBase
    {
        /// <summary>
        /// 这个玩家所在的网关
        /// </summary>
        private GatewayServerForGameClient m_GatewayServerForGameClient;

        /// <summary>
        /// 当前位置
        /// </summary>
        public Vector3 CurrPos;
        
        public PlayerForGameClient(long accountId, GatewayServerForGameClient gatewayServerForWorldClient) : base()
        {
            AccountId = accountId;
            m_GatewayServerForGameClient = gatewayServerForWorldClient;

            HotFixHelper.OnLoadAssembly += InitPlayerForGameClientHandler;
            InitPlayerForGameClientHandler();
        }

        private IPlayerForGameClientHandler m_CurrHandler;

        private void InitPlayerForGameClientHandler()
        {
            if (m_CurrHandler != null)
            {
                //把旧的实例释放掉
                m_CurrHandler.Dispose();
                m_CurrHandler = null;
            }

            m_CurrHandler =
                Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.PlayerForGameClientHandler]) as
                    IPlayerForGameClientHandler;
            m_CurrHandler?.Init(this);

            Console.WriteLine("InitPlayerForGameClientHandler");
        }

        /// <summary>
        /// 发送中转消息
        /// </summary>
        /// <param name="proto"></param>
        public void SendCarryToClient(IProto proto)
        {
            CarryProto carryProto = new CarryProto(AccountId, proto.ProtoId, proto.Category, proto.ToByteArray());
            m_GatewayServerForGameClient.CurrServerClient.ClientSocket.SendMsg(carryProto);
        }
    }
}
