using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
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
            await EnterScene(proto.SceneId, proto.RoleId);
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="protoSceneId"></param>
        /// <param name="protoRoleId"></param>
        private async Task EnterScene(int sceneId, long roleId)
        {
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(sceneId, out var pvpScene))
            {
                //TODO 1.找到场景线 课程里只有一个场景线 实际项目 自己做场景线逻辑 动态选择场景线 比如一个线20人
                //2.拉取玩家数据
                m_PlayerForGameClient.CurrRole = await RoleManager.GetRoleEntityAsync(roleId);
                m_PlayerForGameClient.CurrPos = new UnityEngine.Vector3(m_PlayerForGameClient.CurrRole.PosData.X,
                    m_PlayerForGameClient.CurrRole.PosData.Y, m_PlayerForGameClient.CurrRole.PosData.Z);

                //3.把玩家客户端加入场景线
                pvpScene.DefaultSceneLine.RoleList.AddLast(m_PlayerForGameClient);

                //4.找到玩家所在区域
                int areaId = pvpScene.GetAOIAreaIdByPos(m_PlayerForGameClient.CurrPos);
                if (areaId > 0)
                {
                    Console.WriteLine("角色 RoleId=" + roleId + " 进入区域 areaId=" + areaId);
                    pvpScene.DefaultSceneLine.AOIAreaDic[areaId].RoleClientList.AddLast(m_PlayerForGameClient);
                    m_PlayerForGameClient.CurrAreaId = areaId; //设置当前区域编号
                }
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, 0, $"找不到场景{sceneId}");
            }
        }
    }
}