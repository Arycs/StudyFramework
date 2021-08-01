//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 服务器返回创建角色消息
/// </summary>
public struct WS2C_ReturnCreateRoleProto : IProto
{
    public ushort ProtoCode => 11004;
    public string ProtoEnName => "WS2C_ReturnCreateRole";
    public ProtoCategory Category => ProtoCategory.WorldServer2Client;

    public bool Result; //结果
    public long RoleId; //角色的YFId

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
        ms.WriteLong(RoleId);

        return ms.ToArray();
    }

    public static WS2C_ReturnCreateRoleProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        WS2C_ReturnCreateRoleProto proto = new WS2C_ReturnCreateRoleProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.Result = ms.ReadBool();
        if (!proto.Result)
        {
        }
        proto.RoleId = ms.ReadLong();

        return proto;
    }
}