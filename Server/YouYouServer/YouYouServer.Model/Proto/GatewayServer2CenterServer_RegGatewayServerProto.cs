//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-07-25 14:54:30
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 网关服务器注册到中心服务器
/// </summary>
public struct GatewayServer2CenterServer_RegGatewayServerProto : IProto
{
    public ushort ProtoCode => 10002;
    public string ProtoEnName => "GatewayServer2CenterServer_RegGatewayServer";
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

    public static GatewayServer2CenterServer_RegGatewayServerProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        GatewayServer2CenterServer_RegGatewayServerProto proto = new GatewayServer2CenterServer_RegGatewayServerProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.ServerId = ms.ReadInt();

        return proto;
    }
}