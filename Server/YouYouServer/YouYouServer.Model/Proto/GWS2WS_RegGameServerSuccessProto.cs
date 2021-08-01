//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 网关服务器通知中心服务器注册到游戏服完毕
/// </summary>
public struct GWS2WS_RegGameServerSuccessProto : IProto
{
    public ushort ProtoCode => 10005;
    public string ProtoEnName => "GWS2WS_RegGameServerSuccess";
    public ProtoCategory Category => ProtoCategory.GatewayServer2WorldServer;

    public int ServerId; //服务器编号

    public byte[] ToArray(MMO_MemoryStream ms, bool isChild = false)
    {
        ms.SetLength(0);
        if (!isChild)
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteByte((byte)Category);
        }

        ms.WriteInt(ServerId);

        return ms.ToArray();
    }

    public static GWS2WS_RegGameServerSuccessProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        GWS2WS_RegGameServerSuccessProto proto = new GWS2WS_RegGameServerSuccessProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.ServerId = ms.ReadInt();

        return proto;
    }
}