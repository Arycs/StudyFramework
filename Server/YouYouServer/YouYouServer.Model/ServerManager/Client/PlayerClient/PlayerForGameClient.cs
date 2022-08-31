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
        /// 客户端到网关+网关+游戏服+寻路耗时
        /// </summary>
        public float TotalPingValue;
        
        /// <summary>
        /// 这个玩家(网关+游戏服)的ping值
        /// </summary>
        public float PingValue;
        
        public PlayerForGameClient(long accountId, GatewayServerForGameClient gatewayServerForWorldClient) : base()
        {
            AccountId = accountId;
            m_GatewayServerForGameClient = gatewayServerForWorldClient;
            CurrFsmManager = new RoleFsm.RoleFsm(this);

            HotFixHelper.OnLoadAssembly += InitPlayerForGameClientHandler;
            InitPlayerForGameClientHandler();
        }

        /// <summary>
        /// 这个句柄主要是处理玩家收发消息 业务逻辑等
        /// </summary>
        private IPlayerForGameClientHandler m_CurrHandler;

        private void InitPlayerForGameClientHandler()
        {
            InitFsmHandler();

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
        /// 初始化状态机处理句柄
        /// </summary>
        private void InitFsmHandler()
        {
            if (CurrRoleClientFsmHandler != null)
            {
                //把旧的实例释放掉
                CurrRoleClientFsmHandler.Dispose();
                CurrRoleClientFsmHandler = null;
            }

            CurrRoleClientFsmHandler =
                Activator.CreateInstance(HotFixHelper.HandlerTypeDic[ConstDefine.PlayerClientFsmHandler]) as
                    IRoleClientFsmHandler;
            CurrRoleClientFsmHandler?.Init(this);
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