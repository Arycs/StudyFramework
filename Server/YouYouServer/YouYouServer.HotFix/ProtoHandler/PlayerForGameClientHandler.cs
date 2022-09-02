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
        /// 下线
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_GWS2GS_Offline)]
        private async void OnOffline(byte[] buffer)
        {
            GWS2GS_Offline proto = (GWS2GS_Offline) GWS2GS_Offline.Descriptor.Parser.ParseFrom(buffer);

            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_PlayerForGameClient.CurrSceneId,
                out var pvpScene))
            {
                pvpScene.DefaultSceneLine.AOIAreaDic[m_PlayerForGameClient.CurrAreaId]
                    .RemoveRole(m_PlayerForGameClient, LeaveSceneLineType.Normal);

                //把自己从场景线中移除
                pvpScene.DefaultSceneLine.RoleList.Remove(m_PlayerForGameClient);

                //从游戏服上移除
                GameServerManager.RemovePlayerForGameClient(m_PlayerForGameClient);

                //保存角色信息
                await RoleManager.SaveRoleEntity(m_PlayerForGameClient.CurrRole);
            }
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
                m_PlayerForGameClient.CurrRotationY = m_PlayerForGameClient.CurrRole.RotationY;
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
        private void OnEnterSceneAsync(byte[] buffer)
        {
            GWS2GS_EnterScene proto = (GWS2GS_EnterScene) GWS2GS_EnterScene.Descriptor.Parser.ParseFrom(buffer);
            Console.WriteLine($"进入场景 RoleId => {proto.RoleId}, SceneId=>{proto.SceneId}");
            EnterScene(proto.SceneId, proto.RoleId);
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="sceneId"></param>
        /// <param name="roleId"></param>
        private void EnterScene(int sceneId, long roleId)
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

        /// <summary>
        /// 点击移动消息
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_C2GS_ClickMove)]
        private void ClickMove(byte[] buffer)
        {
            C2GS_ClickMove proto = (C2GS_ClickMove) C2GS_ClickMove.Descriptor.Parser.ParseFrom(buffer);

            UnityEngine.Vector3 targetPos =
                new UnityEngine.Vector3(proto.TargetPos.X, proto.TargetPos.Y, proto.TargetPos.Z);

            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_PlayerForGameClient.CurrSceneId,
                out var pvpScene))
            {
                if (!pvpScene.GetCanArrive(targetPos))
                {
                    //修正前端位置 强拉回去
                    return;
                }
            }

            m_PlayerForGameClient.TargetPos = targetPos;


            Console.WriteLine("ClickMove roleId = {0} pingValue = {1}", m_PlayerForGameClient.RoleId,
                m_PlayerForGameClient.PingValue);

            //进行寻路
            GameServerManager.ConnectNavAgent.GetNavPath(m_PlayerForGameClient.CurrSceneId,
                m_PlayerForGameClient.CurrPos,
                m_PlayerForGameClient.TargetPos, (NS2GS_ReturnNavPath proto) =>
                {
                    if (proto.Path.Count > 1)
                    {
                        //服务器计算路径花费的时间
                        float getNavPathTime = YFDateTimeUtil.GetServerTime() - proto.ServerTime;

                        Console.WriteLine("step 2 RoleId = {0} GetNavPathTime = {1} ChangeState  Run",
                            m_PlayerForGameClient.RoleId, getNavPathTime);

                        m_PlayerForGameClient.PathPoints.Clear();
                        foreach (var item in proto.Path)
                        {
                            m_PlayerForGameClient.PathPoints.Add(new UnityEngine.Vector3(item.X, item.Y, item.Z));
                        }

                        Console.WriteLine("step 3 RoleId = " + m_PlayerForGameClient.RoleId + " ChangeState  Run");
                        if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(
                            m_PlayerForGameClient.CurrSceneId,
                            out var pvpScene))
                        {
                            pvpScene.DefaultSceneLine.AOIAreaDic[m_PlayerForGameClient.CurrAreaId]
                                .RoleMove(m_PlayerForGameClient,
                                    new Vector3() {X = targetPos.x, Y = targetPos.y, Z = targetPos.z},PlayerActionType.ClickMove);
                        }

                        Console.WriteLine("step 4 RoleId = " + m_PlayerForGameClient.RoleId + " ChangeState  Run");
                        m_PlayerForGameClient.TotalPingValue = m_PlayerForGameClient.PingValue + getNavPathTime;
                        m_PlayerForGameClient.CurrFsmManager.RoleFsmRun.OnReSet();
                        m_PlayerForGameClient.CurrFsmManager.ChangeState(Core.RoleState.Run);
                    }
                });
        }

        /// <summary>
        /// 摇杆移动
        /// </summary>
        [HandlerMessage(ProtoIdDefine.Proto_C2GS_JoystickMove)]
        private void JoystickMove(byte[] buffer)
        {
            C2GS_JoystickMove proto =
                (C2GS_JoystickMove) C2GS_JoystickMove.Descriptor.Parser.ParseFrom(buffer);

            UnityEngine.Vector3 currPos =
                new UnityEngine.Vector3(proto.CurrPos.X, proto.CurrPos.Y, proto.CurrPos.Z);

            UnityEngine.Vector3 moveDir =
                new UnityEngine.Vector3(proto.MoveDir.X, proto.MoveDir.Y, proto.MoveDir.Z);

            //模拟出来的目标点
            UnityEngine.Vector3 targetPos = currPos + moveDir.normalized * 0.5f;
            UnityEngine.Vector3 protoTargetPos = currPos + moveDir.normalized * 10;

            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_PlayerForGameClient.CurrSceneId,
                out var pvpScene))
            {
                //是否可以到达
                if (!pvpScene.GetCanArrive(targetPos))
                {
                    //停止移动
                    m_PlayerForGameClient.CurrFsmManager.ChangeState(Core.RoleState.Idle);
                    return;
                }

                pvpScene.DefaultSceneLine.AOIAreaDic[m_PlayerForGameClient.CurrAreaId]
                    .RoleMove(m_PlayerForGameClient,
                        new Vector3() {X = protoTargetPos.x, Y = protoTargetPos.y, Z = protoTargetPos.z}, PlayerActionType.JoystickMove);
            }

            m_PlayerForGameClient.MoveType = PlayerActionType.JoystickMove;

            m_PlayerForGameClient.TargetPos = protoTargetPos;

            Console.WriteLine("JoystickMove roleId = {0} pingValue = {1}", m_PlayerForGameClient.RoleId,
                m_PlayerForGameClient.PingValue);

            //设置这个玩家的总延迟时间
            m_PlayerForGameClient.TotalPingValue = m_PlayerForGameClient.PingValue;
            m_PlayerForGameClient.CurrFsmManager.ChangeState(Core.RoleState.Run);
        }

        /// <summary>
        /// 摇杆停止
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_C2GS_JoystickStop)]
        private void JoystickStop(byte[] buffer)
        {
            C2GS_JoystickStop proto =
                (C2GS_JoystickStop) C2GS_JoystickStop.Descriptor.Parser.ParseFrom(buffer);
            
            //TODO 数据校验
            
            m_PlayerForGameClient.CurrFsmManager.ChangeState(Core.RoleState.Idle);
            
            //修改当前位置
            UnityEngine.Vector3 currPos =
                new UnityEngine.Vector3(proto.CurrPos.X, proto.CurrPos.Y, proto.CurrPos.Z);
            
            m_PlayerForGameClient.CurrPos = currPos;
            m_PlayerForGameClient.CurrRotationY = proto.RotationY;
            
            //写入DB数据
            m_PlayerForGameClient.CurrRole.PosData = new YouYou.Proto.Vector3
            {
                X = m_PlayerForGameClient.CurrPos.x, Y = m_PlayerForGameClient.CurrPos.y,
                Z = m_PlayerForGameClient.CurrPos.z
            };
            m_PlayerForGameClient.CurrRole.RotationY = m_PlayerForGameClient.CurrRotationY;
            
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_PlayerForGameClient.CurrSceneId,
                out var pvpScene))
            {
                pvpScene.DefaultSceneLine.AOIAreaDic[m_PlayerForGameClient.CurrAreaId]
                    .RoleIdle(m_PlayerForGameClient, true);
            }
        }
        
        /// <summary>
        /// 进入AOI区域
        /// </summary>
        /// <param name="buffer"></param>
        [HandlerMessage(ProtoIdDefine.Proto_C2GS_Enter_AOIArea)]
        private void EnterAOI(byte[] buffer)
        {
            var proto = (C2GS_Enter_AOIArea) C2GS_Enter_AOIArea.Descriptor.Parser.ParseFrom(buffer);
            if (GameServerManager.CurrSceneManager.PVPSceneDic.TryGetValue(m_PlayerForGameClient.CurrSceneId,
                out var pvpScene))
            {
                pvpScene.DefaultSceneLine.RoleEnterNewArea(m_PlayerForGameClient, proto.AreaId);
            }
        }
    }
}