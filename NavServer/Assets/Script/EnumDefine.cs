using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 协议分类
/// </summary>
public enum ProtoCategory
{
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
