﻿using System.Collections.Generic;
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
        /// 角色加入区域
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
                item.Status = (int) roleClient.CurrFsmManager.CurrStateType;
            }

            proto.RoleList.Add(item);

            otherPlayer.SendCarryToClient(proto);
        }

        /// <summary>
        /// 移除角色
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
    }
}