using System;
using System.Collections.Generic;
using System.Text;
using YouYou;
using YouYouServer.Core;
using YouYouServer.Model.Entitys;
using YouYouServer.Model.Managers;

namespace YouYouServer.Model.ServerManager
{

    /// <summary>
    /// 中心服务器上的玩家客户端
    /// </summary>
    public class PlayerForWorldClient : PlayerClientBase
    {
        /// <summary>
        /// 当前角色
        /// </summary>
        public RoleEntity CurrRole
        {
            get;private set;
        }

        /// <summary>
        /// 这个玩家所在的网关
        /// </summary>
        private GatewayServerForWorldClient m_GatewayServerForWorldClient;

        public PlayerForWorldClient(long accountId, GatewayServerForWorldClient gatewayServerForWorldClient) : base() {
            AccountId = accountId;
            m_GatewayServerForWorldClient = gatewayServerForWorldClient;
            AddEventListener();
        }

        public override void AddEventListener()
        {
            base.AddEventListener();
            EventDispatcher.AddEventListener(ProtoCodeDef.C2WS_CreateRole, OnCreateRoleAsync);
        }
        public override void RemoveEventListener()
        {
            base.RemoveEventListener();
            EventDispatcher.RemoveEventListener(ProtoCodeDef.C2WS_CreateRole, OnCreateRoleAsync);
        }
        /// <summary>
        /// 客户端发送创建角色消息
        /// </summary>
        /// <param name="buffer"></param>
        private async void OnCreateRoleAsync(byte[] buffer)
        {
            C2WS_CreateRoleProto createRoleProto = C2WS_CreateRoleProto.GetProto(GetProtoMS, buffer);
            CurrRole = await RoleManager.CreateRoleAsync(AccountId, createRoleProto.JobId, createRoleProto.Sex, createRoleProto.NickName);

            WS2C_ReturnCreateRoleProto retProto = new WS2C_ReturnCreateRoleProto();
            if (CurrRole == null)
            {
                //创建角色失败
                retProto.Result = false;
            }
            else
            {
                retProto.Result = true;
                retProto.RoleId = CurrRole.YFId;
            }

            SendCarryToClient(retProto);
        }

        /// <summary>
        /// 发送中转协议到客户端
        /// </summary>
        /// <param name="proto"></param>
        private void SendCarryToClient(IProto proto)
        {
            CarryProto carryProto = new CarryProto(AccountId, proto.ProtoCode, proto.Category, proto.ToArray(SendProtoMS));
            m_GatewayServerForWorldClient.CurrServerClient.ClientSocket.SendMsg(carryProto.ToArray(m_GatewayServerForWorldClient.CurrServerClient.SendProtoMS
                ));
        }
    }
}
