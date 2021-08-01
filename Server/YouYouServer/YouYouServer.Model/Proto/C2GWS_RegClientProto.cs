//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 玩家向网关服务器注册客户端
/// </summary>
public struct C2GWS_RegClientProto : IProto
{
    public ushort ProtoCode => 11001;
    public string ProtoEnName => "C2GWS_RegClient";
    public ProtoCategory Category => ProtoCategory.Client2GatewayServer;

    public long AccountId; //玩家账号

    public byte[] ToArray(MMO_MemoryStream ms, bool isChild = false)
    {
        ms.SetLength(0);
        if (!isChild)
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteByte((byte)Category);
        }

        ms.WriteLong(AccountId);

        return ms.ToArray();
    }

    public static C2GWS_RegClientProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        C2GWS_RegClientProto proto = new C2GWS_RegClientProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.AccountId = ms.ReadLong();

        return proto;
    }
}