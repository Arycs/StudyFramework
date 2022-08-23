using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;

namespace YouYouServer.HotFix
{
    [Handler(ConstDefine.PlayerForGatewayClientHandler)]
    public class PlayerForGatewayClientHandler :IPlayerForGatewayClientHandler
    {
        private PlayerForGatewayClient m_PlayerForGatewayClient;

        private Dictionary<ushort, EventDispatcher.OnActionHandler> m_HandlerMessageDic;
        
        public void Init(PlayerForGatewayClient PlayerForGatewayClient)
        {
            m_PlayerForGatewayClient = PlayerForGatewayClient;
            
            //处理中转协议
            m_PlayerForGatewayClient.ClientSocket.OnCarryProto = OnCarryProto;
            
            AddEventListener();
        }
        
        public void Dispose()
        {
            RemoveEventListener();
        }
        
        #region AddEventListener 添加消息包监听
        /// <summary>
        /// 添加消息包监听
        /// </summary>
        private void AddEventListener()
        {
            m_HandlerMessageDic = new Dictionary<ushort, EventDispatcher.OnActionHandler>();

            //获取这个类上的所有方法
            MethodInfo[] methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            int num = 0;
            while (num < methods.Length)
            {
                MethodInfo methodInfo = methods[num];
                string str = methodInfo.Name;
                object[] customAttributes = methodInfo.GetCustomAttributes(typeof(HandlerMessageAttribute), true);
                foreach (var t in customAttributes)
                {
                    //找到带HandlerMessageAttribute属性标记的类 进行监听
                    if (t is HandlerMessageAttribute handlerMessage)
                    {
                        EventDispatcher.OnActionHandler actionHandler = 
                            (EventDispatcher.OnActionHandler)Delegate.CreateDelegate(typeof(EventDispatcher.OnActionHandler), this, methodInfo);
                        m_HandlerMessageDic[handlerMessage.ProtoId] = actionHandler;
                        m_PlayerForGatewayClient.EventDispatcher.AddEventListener(handlerMessage.ProtoId, actionHandler);
                    }
                }

                num++;
            }
        }
        #endregion

        #region RemoveEventListener 移除消息包监听
        /// <summary>
        /// 移除消息包监听
        /// </summary>
        private void RemoveEventListener()
        {
            using var enumerator = m_HandlerMessageDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                m_PlayerForGatewayClient.EventDispatcher.RemoveEventListener(enumerator.Current.Key, enumerator.Current.Value);
            }
            m_HandlerMessageDic.Clear();
            m_HandlerMessageDic = null;
        }
        #endregion
        
        #region OnC2GWS_RegClient

        [HandlerMessage(ProtoIdDefine.Proto_C2GWS_RegClient)]
        private void OnC2GWS_RegClient(byte[] buffer)
        {
            C2GWS_RegClient proto = (C2GWS_RegClient) C2GWS_RegClient.Descriptor.Parser.ParseFrom(buffer);

            //此处可以加个验证 验证账号合法性
            m_PlayerForGatewayClient.AccountId = proto.AccountId;
            GatewayServerManager.RegisterPlayerClient(m_PlayerForGatewayClient);
            SendRegClientResult();
        }
        
        /// <summary>
        /// 向客户端发送注册结果
        /// </summary>
        private void SendRegClientResult()
        {
            GWS2C_ReturnRegClient proto = new GWS2C_ReturnRegClient();
            proto.Result = true;
            m_PlayerForGatewayClient.ClientSocket.SendMsg(proto);
        }

        #endregion
        
        [HandlerMessage(ProtoIdDefine.Proto_C2GWS_EnterScene_Apply)]
        private void OnC2GWS_EnterScene_Apply(byte[] buffer)
        {
            C2GWS_EnterScene_Apply proto =
                (C2GWS_EnterScene_Apply) C2GWS_EnterScene_Apply.Descriptor.Parser.ParseFrom(buffer);

            //不能进入同一个场景
            if (m_PlayerForGatewayClient.CurrSceneId == proto.SceneId)
            {
                return;
            }

            //根据要进入的场景编号 算出玩家在哪个游戏服务器
            if (ServerConfig.SceneInServerDic.TryGetValue(proto.SceneId, out var sceneConfig))
            {
                GWS2GS_EnterScene_Apply enterSceneAppleProto = new GWS2GS_EnterScene_Apply
                {
                    RoleId = m_PlayerForGatewayClient.RoleId,
                    PrevSceneId = m_PlayerForGatewayClient.CurrSceneId,
                    SceneId = proto.SceneId
                };
                CarrySendToGameServer(sceneConfig.ServerId, enterSceneAppleProto.ProtoId,
                    ProtoCategory.GatewayServer2GameServer, enterSceneAppleProto.ToByteArray());
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, YouYouServer.Common.LogType.RoleLog,
                    "EnterSceneApply Error CurrSceneId {0}", proto.SceneId);
            }
        }
        
        [HandlerMessage(ProtoIdDefine.Proto_C2GWS_EnterScene)]
        private void OnC2GWS_EnterScene(byte[] buffer)
        {
            C2GWS_EnterScene proto =
                (C2GWS_EnterScene) C2GWS_EnterScene.Descriptor.Parser.ParseFrom(buffer);

            //根据要进入的场景编号 算出玩家在哪个游戏服务器
            if (ServerConfig.SceneInServerDic.TryGetValue(proto.SceneId, out var sceneConfig))
            {
                //先离开上一个场景
                if (m_PlayerForGatewayClient.CurrInGameServerId > 0 && m_PlayerForGatewayClient.CurrSceneId > 0)
                {
                    GWS2GS_LeaveScene leaveSceneProto = new GWS2GS_LeaveScene
                    {
                        RoleId = m_PlayerForGatewayClient.RoleId,
                        SceneId = m_PlayerForGatewayClient.CurrSceneId,
                        TargetSceneId = proto.SceneId
                    };
                    CarrySendToGameServer(m_PlayerForGatewayClient.CurrInGameServerId, leaveSceneProto.ProtoId,
                        ProtoCategory.GatewayServer2GameServer, leaveSceneProto.ToByteArray());
                }

                m_PlayerForGatewayClient.CurrInGameServerId = sceneConfig.ServerId;
                m_PlayerForGatewayClient.CurrSceneId = proto.SceneId;

                GWS2GS_EnterScene enterSceneAppleProto = new GWS2GS_EnterScene
                {
                    RoleId = m_PlayerForGatewayClient.RoleId,
                    SceneId = proto.SceneId
                };
                CarrySendToGameServer(m_PlayerForGatewayClient.CurrInGameServerId, enterSceneAppleProto.ProtoId,
                    ProtoCategory.GatewayServer2GameServer, enterSceneAppleProto.ToByteArray());
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, YouYouServer.Common.LogType.RoleLog,
                    "EnterScene Error CurrSceneId {0}", proto.SceneId);
            }
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
                case ProtoCategory.Client2GameServer:
                {
                    // CarrySendToGameServer(m_PlayerForGatewayClient.CurrInGameServerId, protoCode,
                    //     ProtoCategory.GatewayServer2GameServer, buffer);
                }
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
        /// <param name="protoCode">协议编号</param>
        /// <param name="buffer">内容</param>
        public void CarrySendToWorldServer(ushort protoCode, byte[] buffer)
        {
            CarryProto carryProto = new CarryProto(m_PlayerForGatewayClient.AccountId, protoCode, ProtoCategory.Client2WorldServer, buffer);

            //这里拦截进入游戏消息 目的是获取角色编号
            if (ProtoIdDefine.Proto_C2WS_EnterGame == carryProto.CarryProtoId)
            {
                C2WS_EnterGame proto = (C2WS_EnterGame) C2WS_EnterGame.Descriptor.Parser.ParseFrom(carryProto.Buffer);
                m_PlayerForGatewayClient.RoleId = proto.RoleId;
            }

            //通过中心服务器连接代理
            GatewayServerManager.ConnectWorldAgent.TargetServerConnect.ClientSocket.SendMsg(carryProto);
        }

        /// <summary>
        /// 中转发送到游戏服务器的消息
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="protoCode"></param>
        /// <param name="buffer"></param>
        public void CarrySendToGameServer(int serverId, ushort protoCode, ProtoCategory protoCategory, byte[] buffer)
        {
            CarryProto carryProto = new CarryProto(m_PlayerForGatewayClient.AccountId, protoCode, protoCategory, buffer);

            //根据服务器编号 通过代理发送中转消息
            GatewayServerManager.GetGameServerAgent(serverId).TargetServerConnect.ClientSocket.SendMsg(carryProto);
        }
    }
}