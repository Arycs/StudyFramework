using System;
using System.Collections.Generic;
using System.Linq;
using YouYou.Proto;
using YouYouServer.Common;
using YouYouServer.Core;
using YouYouServer.Model;
using YouYouServer.Model.IHandler;
using YouYouServer.Model.ServerManager;

namespace YouYouServer.HotFix
{
    [Handler(ConstDefine.PVPSceneAOIAreaHandler)]
    public class PVPSceneAOIAreaHandler : IPVPSceneAOIArea
    {
        private PVPSceneAOIArea m_PVPSceneAOIArea;

        public void Init(PVPSceneAOIArea pvpSceneAoiArea)
        {
            m_PVPSceneAOIArea = pvpSceneAoiArea;
        }

        public void OnUpdate()
        {
            LinkedListNode<RoleClientBase> curr = m_PVPSceneAOIArea.RoleClientList.First;
            while (curr != null)
            {
                var next = curr.Next;
                curr.Value.OnUpdate();
                curr = next;
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 获取这个区域和关联区域内所有玩家
        /// </summary>
        /// <param name="searchRoleType"></param>
        /// <returns></returns>
        public List<RoleClientBase> GetAllRole(SearchRoleType searchRoleType)
        {
            List<RoleClientBase> retLst = new List<RoleClientBase>();
            foreach (var role in m_PVPSceneAOIArea.RoleClientList)
            {
                if (searchRoleType == SearchRoleType.Player)
                {
                    if (role.CurrRoleType == RoleType.Player)
                    {
                        retLst.Add(role);
                    }
                }
                else if (searchRoleType == SearchRoleType.Monster)
                {
                    if (role.CurrRoleType == RoleType.Monster)
                    {
                        retLst.Add(role);
                    }
                }
                else
                {
                    retLst.Add(role);
                }
            }

            //循环关联区域
            foreach (var item in m_PVPSceneAOIArea.CurrAOIData.ConnectAreaList)
            {
                var area = m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[item];
                foreach (var role in area.RoleClientList)
                {
                    if (searchRoleType == SearchRoleType.Player)
                    {
                        if (role.CurrRoleType == RoleType.Player)
                        {
                            retLst.Add(role);
                        }
                    }
                    else if (searchRoleType == SearchRoleType.Monster)
                    {
                        if (role.CurrRoleType == RoleType.Monster)
                        {
                            retLst.Add(role);
                        }
                    }
                    else
                    {
                        retLst.Add(role);
                    }
                }
            }

            return retLst;
        }

        /// <summary>
        /// 告诉其他玩家 我来了
        /// </summary>
        /// <param name="roleClient"></param>
        /// <param name="otherPlayer"></param>
        public void RoleEnterSceneLine(RoleClientBase roleClient, PlayerForGameClient otherPlayer)
        {
            GS2C_ReturnRoleEnterSceneLine proto = new GS2C_ReturnRoleEnterSceneLine();
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

            proto.RoleList.Add(item);

            otherPlayer.SendCarryToClient(proto);
        }

        /// <summary>
        /// 角色加入区域 这个方法时角色进入场景时调用
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void AddRole(RoleClientBase roleClientBase)
        {
            List<RoleClientBase> players = GetAllRole(SearchRoleType.Player);

            foreach (var role in players)
            {
                RoleEnterSceneLine(roleClientBase, role as PlayerForGameClient);
            }

            m_PVPSceneAOIArea.RoleClientList.AddLast(roleClientBase);
        }


        /// <summary>
        /// 移除角色 这个方法是角色离开场景 如角色下线,离开场景,怪物死亡时调用
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="leaveSceneLineType"></param>
        public void RemoveRole(RoleClientBase roleClientBase, LeaveSceneLineType leaveSceneLineType)
        {
            m_PVPSceneAOIArea.RoleClientList.Remove(roleClientBase);

            List<RoleClientBase> players = GetAllRole(SearchRoleType.Player);
            foreach (var role in players)
            {
                GS2C_ReturnRoleLeaveSceneLine proto = new GS2C_ReturnRoleLeaveSceneLine
                {
                    RoleId = roleClientBase.RoleId,
                    LeaveSceneLineType = leaveSceneLineType
                };
                ((PlayerForGameClient) role).SendCarryToClient(proto);
            }
        }

        /// <summary>
        /// 角色移动
        /// </summary>
        /// <param name="roleClientBase"></param>
        /// <param name="targetPos"></param>
        public void RoleMove(RoleClientBase roleClientBase, Vector3 targetPos)
        {
            List<RoleClientBase> players = GetAllRole(SearchRoleType.Player);
            foreach (var role in players)
            {
                GS2C_ReturnRoleChangeState proto = new GS2C_ReturnRoleChangeState
                {
                    RoleId = roleClientBase.RoleId,
                    Status = (int) RoleState.Run,
                    CurrPos = new Vector3()
                        {X = roleClientBase.CurrPos.x, Y = roleClientBase.CurrPos.y, Z = roleClientBase.CurrPos.z},
                    RotationY = roleClientBase.CurrRotationY,
                    TargetPos = targetPos,
                };
                ((PlayerForGameClient) role).SendCarryToClient(proto);
            }
        }

        /// <summary>
        /// 角色待机
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void RoleIdle(RoleClientBase roleClientBase)
        {
            List<RoleClientBase> players = GetAllRole(SearchRoleType.Player);
            foreach (var role in players)
            {
                GS2C_ReturnRoleChangeState proto = new GS2C_ReturnRoleChangeState
                {
                    RoleId = roleClientBase.RoleId,
                    Status = (int) RoleState.Idle,
                    CurrPos = new Vector3()
                        {X = roleClientBase.CurrPos.x, Y = roleClientBase.CurrPos.y, Z = roleClientBase.CurrPos.z},
                    RotationY = roleClientBase.CurrRotationY,
                };
                ((PlayerForGameClient) role).SendCarryToClient(proto);
            }
        }

        /// <summary>
        /// 检查角色是否跨区域
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void CheckAreaChange(RoleClientBase roleClientBase)
        {
            int areaId = m_PVPSceneAOIArea.CurrSceneLine.OwnerPVPScene.GetAOIAreaIdByPos(roleClientBase.CurrPos);
            if (areaId > 0)
            {
                int oldAreaId = roleClientBase.CurrAreaId;
                if (areaId == oldAreaId)
                {
                    return;
                }

                //从旧区域列表移除
                m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[oldAreaId].RoleClientList.Remove(roleClientBase);

                //加入新区域列表
                m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[areaId].RoleClientList.AddLast(roleClientBase);

                //跨越区域了
                roleClientBase.CurrAreaId = areaId;

                //总区域列表
                List<int> totalAreaList = new List<int>();

                //把旧的区域加入
                totalAreaList.AddRange(m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[oldAreaId].CurrAOIData.AllAreaList);

                //把新的区域也加入
                totalAreaList.AddRange(m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[areaId].CurrAOIData.AllAreaList);

                //计算总区域和新区域的差集  就是要离开的关联区域
                List<int> leaveAreaList = totalAreaList
                    .Except(m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[areaId].CurrAOIData.AllAreaList).ToList();

                //计算总区域和旧区域的差集 就是要进入的关联区域
                List<int> enterAreaList = totalAreaList
                    .Except(m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[oldAreaId].CurrAOIData.AllAreaList).ToList();

                //通知要离开的区域 我离开了
                foreach (var item in leaveAreaList)
                {
                    PVPSceneAOIArea area = m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[item];
                    foreach (var role in area.RoleClientList)
                    {
                        if (roleClientBase.RoleId == role.RoleId)
                        {
                            continue;
                        }

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

                foreach (var item in enterAreaList)
                {
                    PVPSceneAOIArea area = m_PVPSceneAOIArea.CurrSceneLine.AOIAreaDic[item];
                    foreach (var role in area.RoleClientList)
                    {
                        if (roleClientBase.RoleId == role.RoleId)
                        {
                            continue;
                        }

                        if (role.CurrRoleType == RoleType.Player)
                        {
                            Console.WriteLine($"通知玩家 {role.RoleId} ,我来了");
                            RoleEnterSceneLine(roleClientBase, (PlayerForGameClient) role);
                        }
                    }
                }
            }
        }
    }
}