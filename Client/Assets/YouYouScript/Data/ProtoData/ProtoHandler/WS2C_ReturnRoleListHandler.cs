/// Create By 悠游课堂 http://www.u3dol.com 张峻硕13196091831
using YouYou;
using YouYou.Proto;

/// <summary>
/// 服务器返回已有角色消息
/// </summary>
public class WS2C_ReturnRoleListHandler
{
    public static void OnHandler(byte[] buffer)
    {
        WS2C_ReturnRoleList proto = WS2C_ReturnRoleList.Parser.ParseFrom(buffer);

#if DEBUG_LOG_PROTO && DEBUG_MODEL
        GameEntry.Log(LogCategory.Proto, "<color=#00eaff>接收消息:</color><color=#00ff9c>" + proto.ProtoEnName + " " + proto.ProtoId + "</color>");
        GameEntry.Log(LogCategory.Proto, "<color=#c5e1dc>==>>" + proto.ToString() + "</color>");
#endif
        GameEntry.Data.UserDataManager.OnReturnRoleList(proto);
    }
}