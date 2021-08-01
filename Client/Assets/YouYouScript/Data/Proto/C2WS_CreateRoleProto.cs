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
/// 玩家想服务器发送创建角色消息
/// </summary>
public struct C2WS_CreateRoleProto : IProto
{
    public ushort ProtoCode => 11003;
    public string ProtoEnName => "C2WS_CreateRole";
    public ProtoCategory Category => ProtoCategory.Client2WorldServer;

    public byte JobId; //职业编码
    public byte Sex; //性别
    public string NickName; //昵称

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

        ms.WriteByte(JobId);
        ms.WriteByte(Sex);
        ms.WriteUTF8String(NickName);

        byte[] retBuffer = ms.ToArray();
        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return retBuffer;
    }

    public static C2WS_CreateRoleProto GetProto(byte[] buffer, bool isChild = false)
    {
        C2WS_CreateRoleProto proto = new C2WS_CreateRoleProto();

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

        proto.JobId = (byte)ms.ReadByte();
        proto.Sex = (byte)ms.ReadByte();
        proto.NickName = ms.ReadUTF8String();

        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return proto;
    }
}