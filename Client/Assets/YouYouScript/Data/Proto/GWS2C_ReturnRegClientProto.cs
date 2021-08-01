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
/// 网关服务器返回注册客户端结束
/// </summary>
public struct GWS2C_ReturnRegClientProto : IProto
{
    public ushort ProtoCode => 11002;
    public string ProtoEnName => "GWS2C_ReturnRegClient";
    public ProtoCategory Category => ProtoCategory.GatewayServer2Client;

    public bool Result; //结果

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

        byte[] retBuffer = ms.ToArray();
        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return retBuffer;
    }

    public static GWS2C_ReturnRegClientProto GetProto(byte[] buffer, bool isChild = false)
    {
        GWS2C_ReturnRegClientProto proto = new GWS2C_ReturnRegClientProto();

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

        if (isChild)
        {
            GameEntry.Pool.EnqueueClassObject(ms);
        }
        return proto;
    }
}