//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using System.Collections;

/// <summary>
/// 协议编号定义
/// </summary>
public class ProtoCodeDef
{
    /// <summary>
    /// 游戏服务器注册到中心服务器
    /// </summary>
    public const ushort GS2WS_RegGameServer = 10001;

    /// <summary>
    /// 网关服务器注册到中心服务器
    /// </summary>
    public const ushort GWS2WS_RegGatewayServer = 10002;

    /// <summary>
    /// 中心服务器通知网关服务器注册到游戏服
    /// </summary>
    public const ushort WS2GWS_ToRegGameServer = 10003;

    /// <summary>
    /// 网关服务器注册到游戏服务器
    /// </summary>
    public const ushort GWS2GS_RegGatewayServer = 10004;

    /// <summary>
    /// 网关服务器通知中心服务器注册到游戏服完毕
    /// </summary>
    public const ushort GWS2WS_RegGameServerSuccess = 10005;

    /// <summary>
    /// 玩家向网关服务器注册客户端
    /// </summary>
    public const ushort C2GWS_RegClient = 11001;

    /// <summary>
    /// 网关服务器返回注册客户端结束
    /// </summary>
    public const ushort GWS2C_ReturnRegClient = 11002;

    /// <summary>
    /// 玩家想服务器发送创建角色消息
    /// </summary>
    public const ushort C2WS_CreateRole = 11003;

    /// <summary>
    /// 服务器返回创建角色消息
    /// </summary>
    public const ushort WS2C_ReturnCreateRole = 11004;

}
