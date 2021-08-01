//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 服务器返回创建角色消息（工具只生成一次）
/// </summary>
public sealed class WS2C_ReturnCreateRoleHandler
{
    public static void OnWS2C_ReturnCreateRole(byte[] buffer)
    {
        WS2C_ReturnCreateRoleProto proto = WS2C_ReturnCreateRoleProto.GetProto(buffer);
#if DEBUG_LOG_PROTO
        Debug.Log("<color=#00eaff>接收消息:</color><color=#00ff9c>" + proto.ProtoEnName + " " + proto.ProtoCode + "</color>");
        Debug.Log("<color=#c5e1dc>==>>" + JsonUtility.ToJson(proto) + "</color>");
#endif
    }
}