using System;
using System.Collections.Generic;
using System.Reflection;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;

namespace YouYouServer.HotFix
{
    [Handler(ConstDefine.PlayerForGameClientHandler)]
    public class PlayerForGameClientHandler : IPlayerForGameClientHandler
    {
        /// <summary>
        /// 游戏服务器上的玩家客户端
        /// </summary>
        private PlayerForGameClient m_PlayerForGameClient;

        private Dictionary<ushort, EventDispatcher.OnActionHandler> m_HandlerMessageDic;

        public void Init(PlayerForGameClient playerForGameClient)
        {
            m_PlayerForGameClient = playerForGameClient;
            AddEventListener();
        }

        public void Dispose()
        {
            RemoveEventListener();
        }

        /// <summary>
        /// 添加消息监听
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
                            (EventDispatcher.OnActionHandler) Delegate.CreateDelegate(
                                typeof(EventDispatcher.OnActionHandler), this, methodInfo);
                        m_HandlerMessageDic[handlerMessage.ProtoId] = actionHandler;
                        m_PlayerForGameClient.EventDispatcher.AddEventListener(handlerMessage.ProtoId, actionHandler);
                    }
                }

                num++;
            }
        }

        /// <summary>
        /// 移除消息包监听
        /// </summary>
        private void RemoveEventListener()
        {
            using var enumerator = m_HandlerMessageDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                m_PlayerForGameClient.EventDispatcher.RemoveEventListener(enumerator.Current.Key,
                    enumerator.Current.Value);
            }

            m_HandlerMessageDic.Clear();
            m_HandlerMessageDic = null;
        }

        /// <summary>
        /// 离开场景
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_GWS2GS_LeaveScene)]
        private async void OnLeaveSceneAsync(byte[] buffer)
        {
            GWS2GS_LeaveScene proto = (GWS2GS_LeaveScene) GWS2GS_LeaveScene.Descriptor.Parser.ParseFrom(buffer);
            Console.WriteLine(
                $"离开场景 RoleId => {proto.RoleId} ,SceneId => {proto.SceneId}, TargetSceneId =>{proto.TargetSceneId}");
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_GWS2GS_EnterScene)]
        private async void OnEnterSceneAsync(byte[] buffer)
        {
            GWS2GS_EnterScene proto = (GWS2GS_EnterScene) GWS2GS_EnterScene.Descriptor.Parser.ParseFrom(buffer);
            Console.WriteLine($"进入场景 RoleId => {proto.RoleId}, SceneId=>{proto.SceneId}");
        }
    }
}