//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 游戏服务器注册到中心服务器
/// </summary>
public struct GS2WS_RegGameServerProto : IProto
{
    public ushort ProtoCode => 10001;
    public string ProtoEnName => "GS2WS_RegGameServer";
    public ProtoCategory Category => ProtoCategory.GameServer2WorldServer;

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

    public static GS2WS_RegGameServerProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        GS2WS_RegGameServerProto proto = new GS2WS_RegGameServerProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.ServerId = ms.ReadInt();

        return proto;
    }
}