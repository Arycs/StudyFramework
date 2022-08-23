using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;
using Vector3 = YouYou.Proto.Vector3;

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
        /// 申请进入场景
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_GWS2GS_EnterScene_Apply)]
        private async void OnEnterSceneApplyAsync(byte[] buffer)
        {
            GWS2GS_EnterScene_Apply proto =
                (GWS2GS_EnterScene_Apply) GWS2GS_EnterScene_Apply.Descriptor.Parser.ParseFrom(buffer);
            Console.WriteLine($"申请进入场景 RoleId ={proto.RoleId} , SceneId={proto.SceneId}");
            await EnterSceneApply(proto.PrevSceneId, proto.SceneId, proto.RoleId);
        }

        /// <summary>
        /// 申请进入场景
        /// </summary>
        /// <param name="prevSceneId"></param>
        /// <param name="sceneId"></param>
        /// <param name="roleId"></param>
        private async Task EnterSceneApply(int prevSceneId, int sceneId, long roleId)
        {
            //TODO 1. 验证是否可以进入 比如某场景需要80级限制,或特殊要求等 (临时)这里直接返回可以进入

            //2. 拉取玩家数据
            m_PlayerForGameClient.CurrRole = await RoleManager.GetRoleEntityAsync(roleId);
            m_PlayerForGameClient.RoleId = m_PlayerForGameClient.CurrRole.YFId;
            m_PlayerForGameClient.JobId = m_PlayerForGameClient.CurrRole.JobId;
            m_PlayerForGameClient.Sex = m_PlayerForGameClient.CurrRole.Sex;
            m_PlayerForGameClient.NickName = m_PlayerForGameClient.CurrRole.NickName;

            if (sceneId == m_PlayerForGameClient.CurrRole.CurrSceneId)
            {
                m_PlayerForGameClient.CurrPos = new UnityEngine.Vector3(m_PlayerForGameClient.CurrRole.PosData.X,
                    m_PlayerForGameClient.CurrRole.PosData.Y, m_PlayerForGameClient.CurrRole.PosData.Z);
            }
            else
            {
                //TODO 根据目标场景编号,获取传送点信息
            }

            var retProto = new GS2C_ReturnEnterScene_Apply
            {
                SceneId = sceneId,
                Result = true,
                CurrPos = m_PlayerForGameClient.CurrRole.PosData,
                RotationY = m_PlayerForGameClient.CurrRole.RotationY
            };
            m_PlayerForGameClient.SendCarryToClient(retProto);
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
        /// <param name="sceneId"></param>
        /// <param name="roleId"></param>
        private async Task EnterScene(int sceneId, long roleId)
        {
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(sceneId, out var pvpScene))
            {
                //TODO 1.找到场景线 课程里只有一个场景线 实际项目 自己做场景线逻辑 动态选择场景线 比如一个线20人
                //2.把玩家客户端加入场景线
                pvpScene.DefaultSceneLine.RoleList.AddLast(m_PlayerForGameClient);
                m_PlayerForGameClient.CurrSceneId = sceneId;

                //3.找到玩家所在区域
                int areaId = pvpScene.GetAOIAreaIdByPos(m_PlayerForGameClient.CurrPos);
                if (areaId > 0)
                {
                    Console.WriteLine("角色 RoleId=" + roleId + " 进入区域 areaId=" + areaId);
                    PVPSceneAOIArea pvpSceneAoiArea = pvpScene.DefaultSceneLine.AOIAreaDic[areaId];

                    //4. 同步区域角色信息到客户端
                    List<RoleClientBase> retLst = pvpSceneAoiArea.GetAllRole(SearchRoleType.All);

                    GS2C_ReturnSceneLineRoleList sceneLineRoleListProto = new GS2C_ReturnSceneLineRoleList();
                    foreach (var role in retLst)
                    {
                        WS2C_SceneLineRole_DATA item = new WS2C_SceneLineRole_DATA();
                        item.RoleType = role.CurrRoleType;
                        item.RoleId = role.RoleId;
                        item.BaseRoleId = role.BaseRoleId;
                        item.Sex = role.Sex;
                        item.NickName = role.NickName ?? "";

                        var pos = new Vector3 {X = role.CurrPos.x, Y = role.CurrPos.y, Z = role.CurrPos.z};
                        item.CurrPos = pos;
                        item.RotationY = role.CurrRotationY;
                        if (role.CurrFsmManager != null)
                        {
                            item.Status = (int) role.CurrFsmManager.CurrStateType;
                        }

                        sceneLineRoleListProto.RoleList.Add(item);
                    }
                    m_PlayerForGameClient.SendCarryToClient(sceneLineRoleListProto);

                    //5.把自己加入区域列表
                    pvpSceneAoiArea.AddRole(m_PlayerForGameClient);
                    
                    m_PlayerForGameClient.CurrAreaId = areaId; //设置当前区域编号
                    m_PlayerForGameClient.CurrFsmManager.ChangeState(RoleState.Idle);

                }
            }
            else
            {
                LoggerMgr.Log(LoggerLevel.LogError, 0, $"找不到场景{sceneId}");
            }
        }
    }
}