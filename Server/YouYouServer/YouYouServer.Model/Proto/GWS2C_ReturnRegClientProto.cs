//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 网关服务器返回注册客户端结束
/// </summary>
public struct GWS2C_ReturnRegClientProto : IProto
{
    public ushort ProtoCode => 11002;
    public string ProtoEnName => "GWS2C_ReturnRegClient";
    public ProtoCategory Category => ProtoCategory.GatewayServer2Client;

    public bool Result; //结果

    public byte[] ToArray(MMO_MemoryStream ms, bool isChild = false)
    {
        ms.SetLength(0);
        if (!isChild)
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteByte((byte)Category);
        }

        ms.WriteBool(Result);
        if (!Result)
        {
        }

        return ms.ToArray();
    }

    public static GWS2C_ReturnRegClientProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        GWS2C_ReturnRegClientProto proto = new GWS2C_ReturnRegClientProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.Result = ms.ReadBool();
        if (!proto.Result)
        {
        }

        return proto;
    }
}