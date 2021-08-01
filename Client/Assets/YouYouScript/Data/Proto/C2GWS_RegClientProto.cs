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
/// 玩家向网关服务器注册客户端
/// </summary>
public struct C2GWS_RegClientProto : IProto
{
    public ushort ProtoCode => 11001;
    public string ProtoEnName => "C2GWS_RegClient";
    public ProtoCategory Category => ProtoCategory.Client2GatewayServer;

    public long AccountId; //玩家账号

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

        ms.WriteLong(AccountId);

        byte[] retBuffer = ms.ToArray();
        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return retBuffer;
    }

    public static C2GWS_RegClientProto GetProto(byte[] buffer, bool isChild = false)
    {
        C2GWS_RegClientProto proto = new C2GWS_RegClientProto();

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

        proto.AccountId = ms.ReadLong();

        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return proto;
    }
}