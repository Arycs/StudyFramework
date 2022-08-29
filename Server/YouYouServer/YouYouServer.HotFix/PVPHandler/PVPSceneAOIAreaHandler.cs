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
        /// 角色加入区域 这个方法时角色进入场景时调用
        /// </summary>
        /// <param name="roleClientBase"></param>
        public void AddRole(RoleClientBase roleClientBase)
        {
            List<RoleClientBase> players = GetAllRole(SearchRoleType.Player);

            foreach (var role in players)
            {
                m_PVPSceneAOIArea.CurrSceneLine.RoleEnterSceneLine(roleClientBase, role as PlayerForGameClient);
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
    }
}