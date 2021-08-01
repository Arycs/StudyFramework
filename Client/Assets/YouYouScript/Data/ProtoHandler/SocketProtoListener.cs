//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：
//备    注：
//===================================================
using YouYou;

/// <summary>
/// Socket协议监听（工具生成）
/// </summary>
public sealed class SocketProtoListener
{
    /// <summary>
    /// 添加协议监听
    /// </summary>
    public static void AddProtoListener()
    {
        GameEntry.Event.SocketEvent.AddEventListener(ProtoCodeDef.GWS2C_ReturnRegClient, GWS2C_ReturnRegClientHandler.OnGWS2C_ReturnRegClient);
        GameEntry.Event.SocketEvent.AddEventListener(ProtoCodeDef.WS2C_ReturnCreateRole, WS2C_ReturnCreateRoleHandler.OnWS2C_ReturnCreateRole);
    }

    /// <summary>
    /// 移除协议监听
    /// </summary>
    public static void RemoveProtoListener()
    {
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoCodeDef.GWS2C_ReturnRegClient, GWS2C_ReturnRegClientHandler.OnGWS2C_ReturnRegClient);
        GameEntry.Event.SocketEvent.RemoveEventListener(ProtoCodeDef.WS2C_ReturnCreateRole, WS2C_ReturnCreateRoleHandler.OnWS2C_ReturnCreateRole);
    }
}