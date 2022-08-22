/// Create By 悠游课堂 http://www.u3dol.com 张峻硕13196091831
using YouYou;
using YouYou.Proto;

/// <summary>
/// 服务器返回进入场景申请
/// </summary>
public class GS2C_ReturnEnterScene_ApplyHandler
{
    public static void OnHandler(byte[] buffer)
    {
        GS2C_ReturnEnterScene_Apply proto = GS2C_ReturnEnterScene_Apply.Parser.ParseFrom(buffer);

#if DEBUG_LOG_PROTO && DEBUG_MODEL
        GameEntry.Log(LogCategory.Proto, "<color=#00eaff>接收消息:</color><color=#00ff9c>" + proto.ProtoEnName + " " + proto.ProtoId + "</color>");
        GameEntry.Log(LogCategory.Proto, "<color=#c5e1dc>==>>" + proto.ToString() + "</color>");
#endif
        GameEntry.Data.UserDataManager.OnReturnEnterSceneApply(proto);
    }
}