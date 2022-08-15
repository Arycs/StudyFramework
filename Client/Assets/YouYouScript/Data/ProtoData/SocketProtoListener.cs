/// Create By 悠游课堂 http://www.u3dol.com 张峻硕13196091831
using YouYou;

/// <summary>
/// Socket协议监听
/// </summary>
public sealed class SocketProtoListener
{
    /// <summary>
    /// 添加协议监听
    /// </summary>
    public static void AddProtoListener()
    {
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GS2C_ReturnEnterScene_Apply, GS2C_ReturnEnterScene_ApplyHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_WS2C_SceneLineRole_DATA, WS2C_SceneLineRole_DATAHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GS2C_ReturnSceneLineRoleList, GS2C_ReturnSceneLineRoleListHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GS2C_ReturnRoleEnterSceneLine, GS2C_ReturnRoleEnterSceneLineHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GS2C_ReturnRoleLeaveSceneLine, GS2C_ReturnRoleLeaveSceneLineHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GS2C_ReturnRoleChangeState, GS2C_ReturnRoleChangeStateHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GWS2C_ReturnRegClient, GWS2C_ReturnRegClientHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GWS2C_Heartbeat, GWS2C_HeartbeatHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_GWS2C_ReturnOffline, GWS2C_ReturnOfflineHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_WS2C_ReturnCreateRole, WS2C_ReturnCreateRoleHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_WS2C_ReturnRoleInfo, WS2C_ReturnRoleInfoHandler.OnHandler);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoIdDefine.Proto_WS2C_ReturnEnterGameComplete, WS2C_ReturnEnterGameCompleteHandler.OnHandler);
    }

    /// <summary>
    /// 移除协议监听
    /// </summary>
    public static void RemoveProtoListener()
    {
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GS2C_ReturnEnterScene_Apply, GS2C_ReturnEnterScene_ApplyHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_WS2C_SceneLineRole_DATA, WS2C_SceneLineRole_DATAHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GS2C_ReturnSceneLineRoleList, GS2C_ReturnSceneLineRoleListHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GS2C_ReturnRoleEnterSceneLine, GS2C_ReturnRoleEnterSceneLineHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GS2C_ReturnRoleLeaveSceneLine, GS2C_ReturnRoleLeaveSceneLineHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GS2C_ReturnRoleChangeState, GS2C_ReturnRoleChangeStateHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GWS2C_ReturnRegClient, GWS2C_ReturnRegClientHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GWS2C_Heartbeat, GWS2C_HeartbeatHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_GWS2C_ReturnOffline, GWS2C_ReturnOfflineHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_WS2C_ReturnCreateRole, WS2C_ReturnCreateRoleHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_WS2C_ReturnRoleInfo, WS2C_ReturnRoleInfoHandler.OnHandler);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoIdDefine.Proto_WS2C_ReturnEnterGameComplete, WS2C_ReturnEnterGameCompleteHandler.OnHandler);
    }
}