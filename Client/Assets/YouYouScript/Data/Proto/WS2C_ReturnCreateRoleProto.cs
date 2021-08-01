//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;
using YouYou;

[Serializable]
/// <summary>
/// 服务器返回创建角色消息
/// </summary>
public struct WS2C_ReturnCreateRoleProto : IProto
{
    public ushort ProtoCode => 11004;
    public string ProtoEnName => "WS2C_ReturnCreateRole";
    public ProtoCategory Category => ProtoCategory.WorldServer2Client;

    public bool Result; //结果
    public long RoleId; //角色的YFId

    public byte[] ToArray(bool isChild = false)
    {
        MMO_MemoryStream ms = null;
        
        if (!isChild)
        {
            ms = GameEntry.Socket.SocketSendMS;
            ms.SetLength(0);
            ms.WriteUShort(ProtoCode);
            ms.WriteByte((byte)Category);
        }
        else
        {
            ms = GameEntry.Pool.DequeueClassObject<MMO_MemoryStream>();
            ms.SetLength(0);
        }

        ms.WriteBool(Result);
        if (!Result)
        {
        }
        ms.WriteLong(RoleId);

        byte[] retBuffer = ms.ToArray();
        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return retBuffer;
    }

    public static WS2C_ReturnCreateRoleProto GetProto(byte[] buffer, bool isChild = false)
    {
        WS2C_ReturnCreateRoleProto proto = new WS2C_ReturnCreateRoleProto();

        MMO_MemoryStream ms = null;
        if (!isChild)
        {
            ms = GameEntry.Socket.SocketSendMS;
        }
        else
        {
            ms = GameEntry.Pool.DequeueClassObject<MMO_MemoryStream>();
        }
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.Result = ms.ReadBool();
        if (!proto.Result)
        {
        }
        proto.RoleId = ms.ReadLong();

        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return proto;
    }
}