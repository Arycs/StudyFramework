/// Create By 悠游课堂 http://www.u3dol.com 张峻硕13196091831
/// <summary>
/// 协议编号
/// </summary>
public class ProtoIdDefine
{
    /// <summary>
    /// 玩家向服务器发送移动
    /// </summary>
    public const ushort Proto_C2GS_Run = 14001;

    /// <summary>
    /// 玩家向网关服务器注册客户端
    /// </summary>
    public const ushort Proto_C2GWS_RegClient = 10001;

    /// <summary>
    /// 玩家向网关服务器发送进入场景申请消息
    /// </summary>
    public const ushort Proto_C2GWS_EnterScene_Apply = 10002;

    /// <summary>
    /// 玩家向网关服务器发送进入场景消息
    /// </summary>
    public const ushort Proto_C2GWS_EnterScene = 10003;

    /// <summary>
    /// 玩家向服务器发送创建角色消息
    /// </summary>
    public const ushort Proto_C2WS_CreateRole = 12001;

    /// <summary>
    /// 玩家向服务器发送查询已有角色消息
    /// </summary>
    public const ushort Proto_C2WS_GetRoleList = 12002;

    /// <summary>
    /// 玩家向服务器发送进入游戏消息
    /// </summary>
    public const ushort Proto_C2WS_EnterGame = 12003;

    /// <summary>
    /// Vector3
    /// </summary>
    public const ushort Proto_Vector3 = 1;

    /// <summary>
    /// 服务器返回进入场景申请
    /// </summary>
    public const ushort Proto_GS2C_ReturnEnterScene_Apply = 15001;

    /// <summary>
    /// 服务器返回场景线中角色列表消息
    /// </summary>
    public const ushort Proto_WS2C_SceneLineRole_DATA = 13001;

    /// <summary>
    /// 服务器返回场景线中角色列表消息
    /// </summary>
    public const ushort Proto_GS2C_ReturnSceneLineRoleList = 15002;

    /// <summary>
    /// 服务器返回角色进入场景线消息
    /// </summary>
    public const ushort Proto_GS2C_ReturnRoleEnterSceneLine = 15003;

    /// <summary>
    /// 服务器返回角色离开场景线消息
    /// </summary>
    public const ushort Proto_GS2C_ReturnRoleLeaveSceneLine = 15004;

    /// <summary>
    /// 游戏服务器向网关服务器返回心跳消息
    /// </summary>
    public const ushort Proto_GS2GWS_Heartbeat = 21001;

    /// <summary>
    /// 获取路径点
    /// </summary>
    public const ushort Proto_GS2NS_Vector3 = 20001;

    /// <summary>
    /// 获取路径点
    /// </summary>
    public const ushort Proto_GS2NS_GetNavPath = 20002;

    /// <summary>
    /// 游戏服务器注册到中心服务器
    /// </summary>
    public const ushort Proto_GS2WS_RegGameServer = 16001;

    /// <summary>
    /// 网关服务器返回注册客户端结果
    /// </summary>
    public const ushort Proto_GWS2C_ReturnRegClient = 11001;

    /// <summary>
    /// 网关服务器注册到游戏服务器
    /// </summary>
    public const ushort Proto_GWS2GS_RegGatewayServer = 20003;

    /// <summary>
    /// 网关服务器向游戏服务器发送角色离开场景消息
    /// </summary>
    public const ushort Proto_GWS2GS_LeaveScene = 20004;

    /// <summary>
    /// 网关服务器向游戏服务器发送角色进入场景申请消息
    /// </summary>
    public const ushort Proto_GWS2GS_EnterScene_Apply = 20005;

    /// <summary>
    /// 网关服务器向游戏服务器发送角色进入场景消息
    /// </summary>
    public const ushort Proto_GWS2GS_EnterScene = 20006;

    /// <summary>
    /// 网关服务器注册到中心服务器
    /// </summary>
    public const ushort Proto_GWS2WS_RegGatewayServer = 18001;

    /// <summary>
    /// 网关服务器通知中心服务器注册到游戏服完毕
    /// </summary>
    public const ushort Proto_GWS2WS_RegGameServerSuccess = 18002;

    /// <summary>
    /// 获取路径点
    /// </summary>
    public const ushort Proto_NS2GS_Vector3 = 21002;

    /// <summary>
    /// 获取路径点
    /// </summary>
    public const ushort Proto_NS2GS_ReturnNavPath = 21003;

    /// <summary>
    /// 服务器返回创建角色消息
    /// </summary>
    public const ushort Proto_WS2C_ReturnCreateRole = 13002;

    /// <summary>
    /// 服务器返回已有角色消息
    /// </summary>
    public const ushort Proto_WS2C_ReturnRoleList = 13003;

    /// <summary>
    /// 
    /// </summary>
    public const ushort Proto_WS2C_ReturnRoleList_Item = 13004;

    /// <summary>
    /// 服务器返回角色信息
    /// </summary>
    public const ushort Proto_WS2C_ReturnRoleInfo = 13005;

    /// <summary>
    /// 服务器返回进入游戏完毕消息
    /// </summary>
    public const ushort Proto_WS2C_ReturnEnterGameComplete = 13006;

    /// <summary>
    /// 中心服务器通知网关服务器注册到游戏服
    /// </summary>
    public const ushort Proto_WS2GWS_ToRegGameServer = 19001;
}