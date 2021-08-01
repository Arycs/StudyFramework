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
/// 中心服务器通知网关服务器注册到游戏服
/// </summary>
public struct WS2GWS_ToRegGameServerProto : IProto
{
    public ushort ProtoCode => 10003;
    public string ProtoEnName => "WS2GWS_ToRegGameServer";
    public ProtoCategory Category => ProtoCategory.WorldServer2GatewayServer;

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

    public static WS2GWS_ToRegGameServerProto GetProto(byte[] buffer, bool isChild = false)
    {
        WS2GWS_ToRegGameServerProto proto = new WS2GWS_ToRegGameServerProto();

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