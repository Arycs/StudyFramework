using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 协议分类
/// </summary>
public enum ProtoCategory
{
    /// <summary>
    /// 未定义
    /// </summary> 
    None = -1,

    /// <summary>
    /// 客户端->网关服务器
    /// </summary>
    Client2GatewayServer = 0,

    /// <summary>
    /// 网关服务器->客户端
    /// </summary>
    GatewayServer2Client = 1,

    /// <summary>
    /// 客户端->中心服务器
    /// </summary>
    Client2WorldServer = 2,

    /// <summary>
    /// 中心服务器->客户端
    /// </summary>
    WorldServer2Client = 3,

    /// <summary>
    /// 客户端->游戏服务器
    /// </summary>
    Client2GameServer = 4,

    /// <summary>
    /// 游戏服务器->客户端
    /// </summary>
    GameServer2Client = 5,

    /// <summary>
    /// 游戏服务器>中心服务器
    /// </summary>
    GameServer2WorldServer = 6,

    /// <summary>
    /// 中心服务器->游戏服务器
    /// </summary>
    WorldServer2GameServer = 7,

    /// <summary>
    /// 网关服务器>中心服务器
    /// </summary>
    GatewayServer2WorldServer = 8,

    /// <summary>
    /// 中心服务器->网关服务器
    /// </summary>
    WorldServer2GatewayServer = 9,

    /// <summary>
    /// 网关服务器>游戏服务器
    /// </summary>
    GatewayServer2GameServer = 10,

    /// <summary>
    /// 游戏服务器->网关服务器
    /// </summary>
    GameServer2GatewayServer = 11,

    /// <summary>
    /// 游戏服务器->寻路服务器
    /// </summary>
    GameServer2NavServer = 12,

    /// <summary>
    /// 寻路服务器->游戏服务器
    /// </summary>
    NavServer2GameServer = 13,
    
    /// <summary>
    /// 中转协议
    /// </summary>
    CarryProto = 255
}

namespace YouYouServer.Core
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LoggerLevel
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        Log = 0,
        /// <summary>
        /// 警告日志
        /// </summary>
        LogWarning = 1,
        /// <summary>
        /// 错误日志
        /// </summary>
        LogError = 2
    }

    /// <summary>
    /// 数据状态枚举
    /// </summary>
    public enum DataStatus
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 已删除
        /// </summary>
        Delete = 1
    }

    /// <summary>
    /// 服务器定时器运行类型
    /// </summary>
    public enum ServerTimerRunType
    {
        /// <summary>
        /// 一次性
        /// </summary>
        Once,
        /// <summary>
        /// 固定间隔
        /// </summary>
        FixedInterval,
        /// <summary>
        /// 每天
        /// </summary>
        EvertyDay,
        /// <summary>
        /// 每周
        /// </summary>
        EveryWeek,
        /// <summary>
        /// 每月
        /// </summary>
        EveryMonth,
        /// <summary>
        /// 每年
        /// </summary>
        EveryYear
    }

    /// <summary>
    /// 角色状态 
    /// </summary>
    public enum RoleState : sbyte
    {
        /// <summary>
        /// 未设置
        /// </summary>
        None = 0,
        /// <summary>
        /// 待机
        /// </summary>
        Idle = 1,
        /// <summary>
        /// 跑
        /// </summary>
        Run = 2,
        /// <summary>
        /// 攻击
        /// </summary>
        Attack = 3,
        /// <summary>
        /// 受伤
        /// </summary>
        Hurt = 4,
        /// <summary>
        /// 死亡
        /// </summary>
        Die = 5,
    }

    public enum SearchRoleType : byte
    {
        /// <summary>
        /// 所有角色
        /// </summary>
        All = 0,
        /// <summary>
        /// 玩家
        /// </summary>
        Player = 1,
        /// <summary>
        /// 怪
        /// </summary>
        Monster = 2,
    }
}
