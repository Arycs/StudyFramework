//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 中心服务器通知网关服务器注册到游戏服
/// </summary>
public struct WS2GWS_ToRegGameServerProto : IProto
{
    public ushort ProtoCode => 10003;
    public string ProtoEnName => "WS2GWS_ToRegGameServer";
    public ProtoCategory Category => ProtoCategory.WorldServer2GatewayServer;

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

    public static WS2GWS_ToRegGameServerProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        WS2GWS_ToRegGameServerProto proto = new WS2GWS_ToRegGameServerProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.ServerId = ms.ReadInt();

        return proto;
    }
}