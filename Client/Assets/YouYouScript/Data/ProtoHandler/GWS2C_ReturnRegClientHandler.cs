//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网关服务器返回注册客户端结束（工具只生成一次）
/// </summary>
public sealed class GWS2C_ReturnRegClientHandler
{
    public static void OnGWS2C_ReturnRegClient(byte[] buffer)
    {
        GWS2C_ReturnRegClientProto proto = GWS2C_ReturnRegClientProto.GetProto(buffer);
#if DEBUG_LOG_PROTO
        Debug.Log("<color=#00eaff>接收消息:</color><color=#00ff9c>" + proto.ProtoEnName + " " + proto.ProtoCode + "</color>");
        Debug.Log("<color=#c5e1dc>==>>" + JsonUtility.ToJson(proto) + "</color>");
#endif
    }
}