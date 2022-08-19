using System;
using System.Net.Sockets;
using Google.Protobuf;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Core.Common;

namespace YouYouServer.Model
{
    /// <summary>
    /// 玩家客户端
    /// </summary>
    public class PlayerForGatewayClient : PlayerClientBase
    {
        /// <summary>
        /// 当前角色编号
        /// </summary>
        private long m_RoleId = 0;

        /// <summary>
        /// 当前角色在哪个游戏服
        /// </summary>
        private int m_CurrInGameServerId = 0;

        /// <summary>
        /// 当前角色在哪个场景
        /// </summary>
        private int m_CurrSceneId;

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
                //Todo 通知中心服务器和游戏服 玩家下线了 

                Dispose();
            };
            //处理中转协议
            ClientSocket.OnCarryProto = OnCarryProto;

            AddEventListener();
        }

        /// <summary>
        /// 监听玩家客户端的消息
        /// </summary>
        public void AddEventListener()
        {
            EventDispatcher.AddEventListener(ProtoIdDefine.Proto_C2GWS_RegClient, OnC2GWS_RegClient);
            EventDispatcher.AddEventListener(ProtoIdDefine.Proto_C2GWS_EnterScene, OnC2GWS_EnterScene);
        }

        /// <summary>
        /// 移除玩家客户端的消息
        /// </summary>
        public void RemoveEventListener()
        {
            EventDispatcher.RemoveEventListener(ProtoIdDefine.Proto_C2GWS_RegClient, OnC2GWS_RegClient);
            EventDispatcher.RemoveEventListener(ProtoIdDefine.Proto_C2GWS_EnterScene, OnC2GWS_EnterScene);
        }

        #region OnC2GWS_RegClient

        /// <summary>
        /// 注册客户端
        /// </summary>
        /// <param name="buffer"></param>
        private void OnC2GWS_RegClient(byte[] buffer)
        {
            C2GWS_RegClient proto = (C2GWS_RegClient) C2GWS_RegClient.Descriptor.Parser.ParseFrom(buffer);

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
            GWS2C_ReturnRegClient proto = new GWS2C_ReturnRegClient();
            proto.Result = true;
            ClientSocket.SendMsg(proto);
        }

        #endregion

        #region OnC2GWS_EnterScene

        /// <summary>
        /// 进入场景消息
        /// </summary>
        /// <param name="buffer"></param>
        private void OnC2GWS_EnterScene(byte[] buffer)
        {
            C2GWS_EnterScene proto = (C2GWS_EnterScene) C2GWS_EnterScene.Descriptor.Parser.ParseFrom(buffer);

            //不能进入同一场景
            if (m_CurrSceneId == proto.SceneId)
            {
                //TODO 如果进入这里,应该考虑给客户端发送一条消息,告诉他已经在了.
                return;
            }

            //根据要进入的场景编号 算出玩家在哪个游戏服务器
            if (ServerConfig.SceneInServerDic.TryGetValue(proto.SceneId, out var sceneConfig))
            {
                //先离开上一个场景
                if (m_CurrInGameServerId > 0 && m_CurrSceneId > 0)
                {
                    GWS2GS_LeaveScene leaveSceneProto = new GWS2GS_LeaveScene
                    {
                        RoleId = m_RoleId, SceneId = m_CurrSceneId, TargetSceneId = proto.SceneId
                    };
                    CarrySendToGameServer(m_CurrInGameServerId, leaveSceneProto.ProtoId,
                        ProtoCategory.GatewayServer2GameServer, leaveSceneProto.ToByteArray());
                }

                m_CurrInGameServerId = sceneConfig.ServerId;
                m_CurrSceneId = proto.SceneId;

                GWS2GS_EnterScene enterSceneProto = new GWS2GS_EnterScene();
                enterSceneProto.RoleId = m_RoleId;
                enterSceneProto.SceneId = proto.SceneId;
                CarrySendToGameServer(m_CurrInGameServerId, enterSceneProto.ProtoId,
                    ProtoCategory.GatewayServer2GameServer, enterSceneProto.ToByteArray());
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, Common.LogType.RoleLog,
                    $"EnterScene Error CurrSceneId {proto.SceneId}");
            }
        }

        #endregion

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
            CarryProto carryProto = new CarryProto(AccountId, protoCode, ProtoCategory.Client2WorldServer, buffer);

            //这里拦截进入游戏消息, 目的是获取角色编号
            if (ProtoIdDefine.Proto_C2WS_EnterGame == carryProto.CarryProtoId)
            {
                C2WS_EnterGame proto = (C2WS_EnterGame) C2WS_EnterGame.Descriptor.Parser.ParseFrom(carryProto.Buffer);
                m_RoleId = proto.RoleId;
            }

            //通过中心服务器连接代理
            GatewayServerManager.ConnectWorldAgent.TargetServerConnect.ClientSocket.SendMsg(carryProto);
        }

        /// <summary>
        /// 中转发送到游戏服务器的消息
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="protoCode"></param>
        /// <param name="protoCategory"></param>
        /// <param name="buffer"></param>
        private void CarrySendToGameServer(int serverId, ushort protoCode, ProtoCategory protoCategory, byte[] buffer)
        {
            CarryProto carryProto = new CarryProto(AccountId, protoCode, protoCategory, buffer);
            //根据服务器编号 通过代理发送中转消息
            GatewayServerManager.GetGameServerAgent(serverId).TargetServerConnect.ClientSocket.SendMsg(carryProto);
        }
    }
}