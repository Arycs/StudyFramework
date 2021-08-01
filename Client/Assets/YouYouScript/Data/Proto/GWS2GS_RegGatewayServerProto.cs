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
/// 网关服务器注册到游戏服务器
/// </summary>
public struct GWS2GS_RegGatewayServerProto : IProto
{
    public ushort ProtoCode => 10004;
    public string ProtoEnName => "GWS2GS_RegGatewayServer";
    public ProtoCategory Category => ProtoCategory.GatewayServer2GameServer;

    public int ServerId; //服务器编号

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

        ms.WriteInt(ServerId);

        byte[] retBuffer = ms.ToArray();
        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return retBuffer;
    }

    public static GWS2GS_RegGatewayServerProto GetProto(byte[] buffer, bool isChild = false)
    {
        GWS2GS_RegGatewayServerProto proto = new GWS2GS_RegGatewayServerProto();

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

        proto.ServerId = ms.ReadInt();

        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return proto;
    }
}