using System;
using System.Collections.Generic;
using System.Reflection;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Common.DBData;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.DataTable;

namespace YouYouServer.HotFix
{
    [Handler(ConstDefine.PlayerForWorldClientHandler)]
    public class PlayerForWorldClientHandler : IPlayerForWorldClientHandler
    {
        /// <summary>
        /// 中心服务器上的玩家客户端
        /// </summary>
        private PlayerForWorldClient m_PlayerForWorldClient;

        private Dictionary<ushort, EventDispatcher.OnActionHandler> m_HandlerMessageDic;

        public void Init(PlayerForWorldClient playerForWorldClient)
        {
            m_PlayerForWorldClient = playerForWorldClient;
            AddEventListener();
        }

        #region AddEventListener 添加消息包监听

        /// <summary>
        /// 添加消息包监听
        /// </summary>
        public void AddEventListener()
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
                    HandlerMessageAttribute handlerMessage = t as HandlerMessageAttribute;
                    if (null != handlerMessage)
                    {
                        EventDispatcher.OnActionHandler actionHandler =
                            (EventDispatcher.OnActionHandler) Delegate.CreateDelegate(
                                typeof(EventDispatcher.OnActionHandler), this, methodInfo);
                        m_HandlerMessageDic[handlerMessage.ProtoId] = actionHandler;
                        m_PlayerForWorldClient.EventDispatcher.AddEventListener(handlerMessage.ProtoId, actionHandler);
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
                m_PlayerForWorldClient.EventDispatcher.RemoveEventListener(enumerator.Current.Key,
                    enumerator.Current.Value);
            }

            m_HandlerMessageDic.Clear();
            m_HandlerMessageDic = null;
        }

        #endregion

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_GWS2WS_Offline)]
        private void OnOffline(byte[] buffer)
        {
            GWS2WS_Offline proto = (GWS2WS_Offline) GWS2WS_Offline.Descriptor.Parser.ParseFrom(buffer);
            WorldServerManager.RemovePlayerClient(proto.AccountId);
        }


        [HandlerMessage(ProtoIdDefine.Proto_C2WS_GetRoleList)]
        private async void OnGetRoleListAsync(byte[] buffer)
        {
            List<RoleBriefEntity> lst = await RoleManager.GetRoleListAsync(m_PlayerForWorldClient.AccountId);

            WS2C_ReturnRoleList retProto = new WS2C_ReturnRoleList();
            foreach (var item in lst)
            {
                retProto.RoleList.Add(new WS2C_ReturnRoleList.Types.WS2C_ReturnRoleList_Item()
                {
                    RoleId = item.RoleId,
                    JobId = item.JobId,
                    NickName = item.NickName,
                    Sex = item.Sex,
                    Level = item.Level
                });
            }
            m_PlayerForWorldClient.SendCarryToClient(retProto);
        }


        /// <summary>
        /// 客户端发送创建角色消息
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_C2WS_CreateRole)]
        private async void OnCreateRoleAsync(byte[] buffer)
        {
            C2WS_CreateRole createRoleProto = (C2WS_CreateRole) C2WS_CreateRole.Descriptor.Parser.ParseFrom(buffer);
            m_PlayerForWorldClient.CurrRole = await RoleManager.CreateRoleAsync(m_PlayerForWorldClient.AccountId,
                (byte) createRoleProto.JobId, (byte) createRoleProto.Sex, createRoleProto.NickName);

            WS2C_ReturnCreateRole retProto = new WS2C_ReturnCreateRole();
            if (m_PlayerForWorldClient.CurrRole == null)
            {
                //创建角色失败
                retProto.Result = false;
                retProto.RoleId = -100;
            }
            else
            {
                //创建角色成功
                retProto.Result = true;
                retProto.RoleId = m_PlayerForWorldClient.CurrRole.YFId;
            }

            m_PlayerForWorldClient.SendCarryToClient(retProto);
        }

        [HandlerMessage(ProtoIdDefine.Proto_C2WS_EnterGame)]
        private async void OnEnterGameAsync(byte[] buffer)
        {
            C2WS_EnterGame proto =(C2WS_EnterGame) C2WS_EnterGame.Descriptor.Parser.ParseFrom(buffer) as C2WS_EnterGame;
            //拿到这个角色的信息
            RoleEntity roleEntity = await RoleManager.GetRoleEntityAsync(proto.RoleId);
            
            //给客户端发送角色信息 
            WS2C_ReturnRoleInfo retRoleInfoProto = new WS2C_ReturnRoleInfo();
            retRoleInfoProto.RoleId = roleEntity.YFId;
            retRoleInfoProto.JobId = roleEntity.JobId;
            retRoleInfoProto.Sex = roleEntity.Sex;
            retRoleInfoProto.NickName = roleEntity.NickName;
            retRoleInfoProto.Level = roleEntity.Level;
            retRoleInfoProto.CurrSceneId = roleEntity.CurrSceneId;
            if (roleEntity.PosData == null)
            {
                //读取第一个场景的玩家出生点
                DTSys_SceneEntity dtSysScene = DataTableManager.Sys_SceneList.GetDic(3);
                roleEntity.PosData = new Vector3
                {
                    X = dtSysScene.PlayerBornPos_1, Y = dtSysScene.PlayerBornPos_2, Z = dtSysScene.PlayerBornPos_3
                };
            }
            retRoleInfoProto.CurrPos = roleEntity.PosData;
            retRoleInfoProto.Level = roleEntity.Level;
            retRoleInfoProto.RotationY = roleEntity.RotationY;
            m_PlayerForWorldClient.SendCarryToClient(retRoleInfoProto);
            
            //告诉客户端 进入游戏完毕
            WS2C_ReturnEnterGameComplete retEnterGameComplete = new WS2C_ReturnEnterGameComplete();
            m_PlayerForWorldClient.SendCarryToClient(retEnterGameComplete);
        }


        public void Dispose()
        {
            RemoveEventListener();
        }
    }
}