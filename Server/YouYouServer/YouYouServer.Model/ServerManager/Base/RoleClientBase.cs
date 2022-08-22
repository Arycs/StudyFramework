using System;
using System.Collections.Generic;
using System.Text;
using YouYou.Proto;
using YouYouServer.Model.IHandler;

namespace YouYouServer.Model.ServerManager
{
    /// <summary>
    /// 角色客户端基类
    /// </summary>
    public abstract class RoleClientBase
    {
        public RoleClientBase() { }

        /// <summary>
        /// 角色编号
        /// </summary>
        public long RoleId;

        /// <summary>
        /// 职业ID 或者是角色ID ,用来给客户端获取配置中的信息
        /// </summary>
        public byte JobId;

        /// <summary>
        /// 怪编号
        /// </summary>
        public int MonsterId;

        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;

        /// <summary>
        /// 角色昵称 或者 怪物名称(客户端可能会自行读表)
        /// </summary>
        public string NickName;
        
        /// <summary>
        /// 当前位置
        /// </summary>
        public UnityEngine.Vector3 CurrPos;

        /// <summary>
        /// 当前旋转
        /// </summary>
        public float CurrRotationY;

        /// <summary>
        /// 当前所在场景编号
        /// </summary>
        public int CurrSceneId;
        
        /// <summary>
        /// 当前所在区域编号
        /// </summary>
        public int CurrAreaId;
        
        /// <summary>
        /// 当前角色客户端处理句柄
        /// </summary>
        public IRoleClientHandler CurrRoleClientHandler;

        /// <summary>
        /// 当前的状态机管理器
        /// </summary>
        public RoleFsm.RoleFsm CurrFsmManager;

        /// <summary>
        /// 当前角色类型
        /// </summary>
        public abstract RoleType CurrRoleType { get; }
        
        /// <summary>
        /// 基础角色编号
        /// </summary>
        public int BaseRoleId => CurrRoleType == RoleType.Player ? JobId : MonsterId;
        public void OnUpdate()
        {
            CurrRoleClientHandler?.OnUpdate();
            CurrFsmManager?.OnUpdate();
        }
    }
}
