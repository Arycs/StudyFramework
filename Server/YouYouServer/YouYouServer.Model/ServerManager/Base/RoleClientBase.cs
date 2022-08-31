using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
        /// 职业ID 
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
        /// 当前角色客户端状态机处理句柄
        /// </summary>
        public IRoleClientFsmHandler CurrRoleClientFsmHandler;

        /// <summary>
        /// 当前的状态机管理器
        /// </summary>
        public RoleFsm.RoleFsm CurrFsmManager;

        //移动目标点
        public UnityEngine.Vector3 TargetPos;
        
        /// <summary>
        /// 当前角色类型
        /// </summary>
        public abstract RoleType CurrRoleType { get; }
        
        /// <summary>
        /// 基础角色编号
        /// </summary>
        public int BaseRoleId => CurrRoleType == RoleType.Player ? JobId : MonsterId;

        #region  移动相关

        /// <summary>
        /// 路径点
        /// </summary>
        public List<UnityEngine.Vector3> PathPoints;

        /// <summary>
        /// 客户端到网关+网关+游戏服+寻路耗时
        /// </summary>
        public float TotalPingValue;

        /// <summary>
        /// 移动的距离
        /// </summary>
        public float MoveDis;

        /// <summary>
        /// 修正速度
        /// </summary>
        public float ModifyRunSpeed = 10;

        /// <summary>
        /// 跑需要的时间
        /// </summary>
        public float RunNeedTime = 0;
        
        /// <summary>
        /// 当前路径点索引
        /// </summary>
        public int CurrWayPointIndex = 0;

        /// <summary>
        /// 移动开始点
        /// </summary>
        public UnityEngine.Vector3 RunBeginPos;

        /// <summary>
        /// 移动结束点
        /// </summary>
        public UnityEngine.Vector3 RunEndPos;

        /// <summary>
        /// 移动方向
        /// </summary>
        public UnityEngine.Vector3 RunDir;
        
        /// <summary>
        /// 移动速度
        /// </summary>
        public float RunSpeed = 10f;

        /// <summary>
        /// 移动时间
        /// </summary>
        public float RunTime = 0;

        /// <summary>
        /// 转身完毕
        /// </summary>
        public bool TurnComplete = false;
        
        #endregion

        public void OnUpdate()
        {
            CurrRoleClientFsmHandler?.OnUpdate();
            CurrFsmManager?.OnUpdate();
        }
    }
}
