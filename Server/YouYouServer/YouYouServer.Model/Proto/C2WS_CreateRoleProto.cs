//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-08-01 18:07:42
//备    注：
//===================================================
using YouYou;
using YouYouServer.Core;
using YouYouServer.Core.Common;

/// <summary>
/// 玩家想服务器发送创建角色消息
/// </summary>
public struct C2WS_CreateRoleProto : IProto
{
    public ushort ProtoCode => 11003;
    public string ProtoEnName => "C2WS_CreateRole";
    public ProtoCategory Category => ProtoCategory.Client2WorldServer;

    public byte JobId; //职业编码
    public byte Sex; //性别
    public string NickName; //昵称

    public byte[] ToArray(MMO_MemoryStream ms, bool isChild = false)
    {
        ms.SetLength(0);
        if (!isChild)
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteByte((byte)Category);
        }

        ms.WriteByte(JobId);
        ms.WriteByte(Sex);
        ms.WriteUTF8String(NickName);

        return ms.ToArray();
    }

    public static C2WS_CreateRoleProto GetProto(MMO_MemoryStream ms, byte[] buffer)
    {
        C2WS_CreateRoleProto proto = new C2WS_CreateRoleProto();
        ms.SetLength(0);
        ms.Write(buffer, 0, buffer.Length);
        ms.Position = 0;

        proto.JobId = (byte)ms.ReadByte();
        proto.Sex = (byte)ms.ReadByte();
        proto.NickName = ms.ReadUTF8String();

        return proto;
    }
}