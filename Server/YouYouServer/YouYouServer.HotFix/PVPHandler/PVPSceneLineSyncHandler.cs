using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using YouYou.Proto;
using YouYouServer.Commmon;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;
using YouYouServer.Model.ServerManager.Client.MonsterClient;

namespace YouYouServer.HotFix.PVPHandler
{
    [Handler(ConstDefine.PVPSceneLineSyncHandler)]
    public class PVPSceneLineSyncHandler : IPVPSceneLineSyncHandler
    {
        private PVPSceneLine m_CurrPVPSceneLine;

        /// <summary>
        /// 上一次的执行时间
        /// </summary>
        private float m_PrevTime = 0;

        public void Init(PVPSceneLine pvpSceneLine)
        {
            m_CurrPVPSceneLine = pvpSceneLine;
            m_PrevTime = TimerManager.time;
        }

        public void SyncStatus()
        {
            //休眠 20 毫秒 1000/20 = 50帧 
            //TODO 帧率修改在这里 1000/X = Y帧
            Thread.Sleep(20);

            //设置到上一帧的时间
            m_CurrPVPSceneLine.Deltatime = TimerManager.time - m_PrevTime;
            m_PrevTime = TimerManager.time;

            //具体逻辑在这里写
            foreach (var item in m_CurrPVPSceneLine.AOIAreaDic)
            {
                item.Value.OnUpdate();
            }

            foreach (var item in m_CurrPVPSceneLine.SpawnMonsterPointDic)
            {
                item.Value.OnUpdate();
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 告诉其他玩家 我来了
        /// </summary>
        /// <param name="roleClient"></param>
        /// <param name="otherPlayer"></param>
        public void RoleEnterSceneLine(RoleClientBase roleClient, PlayerForGameClient otherPlayer)
        {
            GS2C_ReturnRoleEnterSceneLine proto = new GS2C_ReturnRoleEnterSceneLine();

            proto.RoleList.Add(GetSceneLineRoleData(roleClient));

            otherPlayer.SendCarryToClient(proto);
        }

        /// <summary>
        /// 封装场景线角色数据
        /// </summary>
        /// <param name="roleClient"></param>
        /// <returns></returns>
        private WS2C_SceneLineRole_DATA GetSceneLineRoleData(RoleClientBase roleClient)
        {
            WS2C_SceneLineRole_DATA item = new WS2C_SceneLineRole_DATA
            {
                RoleType = roleClient.CurrRoleType,
                RoleId = roleClient.RoleId,
                BaseRoleId = roleClient.BaseRoleId,
                Sex = roleClient.Sex,
                NickName = roleClient.NickName ?? ""
            };
            
            var pos = new Vector3 {X = roleClient.CurrPos.x, Y = roleClient.CurrPos.y, Z = roleClient.CurrPos.z};
            item.CurrPos = pos;
            item.RotationY = roleClient.CurrRotationY;
            if (roleClient.CurrFsmManager != null)
            {
                Console.WriteLine($"CurrStateType ==> {roleClient.CurrFsmManager.CurrStateType}");
                item.Status = (int) roleClient.CurrFsmManager.CurrStateType;
                if (item.Status == (int) RoleState.Run)
                {
                    //如果这个角色正在跑 发送移动目标点
                    item.TargetPos = new Vector3()
                        {X = roleClient.TargetPos.x, Y = roleClient.TargetPos.y, Z = roleClient.TargetPos.z};
                }
            }

            return item;
        }

        /// <summary>
        /// 检查角色是否跨区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void CheckAreaChange(RoleClientBase roleClientBase)
        {
            int areaId = m_CurrPVPSceneLine.OwnerPVPScene.GetAOIAreaIdByPos(roleClientBase.CurrPos);
            if (areaId > 0)
            {
                int oldAreaId = roleClientBase.CurrAreaId;
                if (areaId == oldAreaId)
                {
                    return;
                }

                EnterNewArea(roleClientBase, oldAreaId, areaId);
            }
            else
            {
                Console.WriteLine($"AreaId 不存在");
            }
        }

        /// <summary>
        /// 进入新区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="oldAreaId"></param>
        /// <param name="areaId"></param>
        private void EnterNewArea(RoleClientBase roleClientBase, int oldAreaId, int areaId)
        {
            if (oldAreaId == areaId)
            {
                return;
            }

            //从旧区域列表移除
            m_CurrPVPSceneLine.AOIAreaDic[oldAreaId].RoleClientList.Remove(roleClientBase);

            //加入新区域列表
            m_CurrPVPSceneLine.AOIAreaDic[areaId].RoleClientList.AddLast(roleClientBase);

            //跨越区域了
            roleClientBase.CurrAreaId = areaId;

            //总区域列表
            List<int> totalAreaList = new List<int>();

            //把旧的区域加入
            totalAreaList.AddRange(m_CurrPVPSceneLine.AOIAreaDic[oldAreaId].CurrAOIData.AllAreaList);

            //把新的区域也加入
            totalAreaList.AddRange(m_CurrPVPSceneLine.AOIAreaDic[areaId].CurrAOIData.AllAreaList);

            //计算总区域和新区域的差集  就是要离开的关联区域
            List<int> leaveAreaList = totalAreaList
                .Except(m_CurrPVPSceneLine.AOIAreaDic[areaId].CurrAOIData.AllAreaList).ToList();

            //计算总区域和旧区域的差集 就是要进入的关联区域
            List<int> enterAreaList = totalAreaList
                .Except(m_CurrPVPSceneLine.AOIAreaDic[oldAreaId].CurrAOIData.AllAreaList).ToList();

            //离开的角色列表
            List<long> leaveRoleList = new List<long>();
            
            //通知要离开的区域 我离开了
            foreach (var item in leaveAreaList)
            {
                PVPSceneAOIArea area = m_CurrPVPSceneLine.AOIAreaDic[item];
                foreach (var role in area.RoleClientList)
                {
                    if (roleClientBase.RoleId == role.RoleId)
                    {
                        continue;
                    }

                    leaveRoleList.Add(role.RoleId);
                    
                    if (role.CurrRoleType == RoleType.Player)
                    {
                        GS2C_ReturnRoleLeaveSceneLine proto = new GS2C_ReturnRoleLeaveSceneLine
                        {
                            RoleId = roleClientBase.RoleId
                        };
                        Console.WriteLine($"通知玩家 {role.RoleId}, 玩家离开");
                        ((PlayerForGameClient) role).SendCarryToClient(proto);
                    }
                }
            }

            if (roleClientBase.CurrRoleType == RoleType.Player && leaveRoleList.Count > 0)
            {
                //告诉我 有人离开了场景线
                foreach (var roleId in leaveRoleList)
                {
                    GS2C_ReturnRoleLeaveSceneLine proto = new GS2C_ReturnRoleLeaveSceneLine()
                    {
                        RoleId =  roleId
                    };
                    ((PlayerForGameClient)roleClientBase).SendCarryToClient(proto);
                }
            }

            List<RoleClientBase> enterRoleList = new List<RoleClientBase>();
            
            foreach (var item in enterAreaList)
            {
                PVPSceneAOIArea area = m_CurrPVPSceneLine.AOIAreaDic[item];
                foreach (var role in area.RoleClientList)
                {
                    if (roleClientBase.RoleId == role.RoleId)
                    {
                        continue;
                    }

                    enterRoleList.Add(role);
                    
                    if (role.CurrRoleType == RoleType.Player)
                    {
                        Console.WriteLine($"通知玩家 {role.RoleId} ,我来了");
                        RoleEnterSceneLine(roleClientBase, (PlayerForGameClient) role);
                    }
                }
            }

            if (roleClientBase.CurrRoleType == RoleType.Player && enterRoleList .Count > 0)
            {
                //告诉我 有人进入了场景线
                GS2C_ReturnRoleEnterSceneLine protoEnterSceneLine = new GS2C_ReturnRoleEnterSceneLine();
                foreach (var role in enterRoleList)
                {
                    protoEnterSceneLine.RoleList.Add(GetSceneLineRoleData(role));
                }
                ((PlayerForGameClient)roleClientBase).SendCarryToClient(protoEnterSceneLine);
            }
        }

        /// <summary>
        /// 角色进入新区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="areaId"></param>
        public void RoleEnterNewArea(RoleClientBase roleClientBase, int areaId)
        {
            EnterNewArea(roleClientBase,roleClientBase.CurrAreaId,areaId);
        }
    }
}