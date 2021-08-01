using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core;
using YouYouServer.Core.Common;

namespace YouYouServer.Core
{
    /// <summary>
    /// 客户端发给网关服务器时使用的中转协议（运载别的协议使用）
    /// </summary>
    public class CarryProto
    {
        /// <summary>
        /// 协议分类
        /// </summary>
        public ProtoCategory Category = ProtoCategory.CarryProto;

        /// <summary>
        /// 玩家账号
        /// </summary>
        public long AccountId;

        /// <summary>
        /// 承运的协议号
        /// </summary>
        public ushort CarryProtoCode;

        /// <summary>
        /// 承运协议分类（也就是承运别的协议的分类）
        /// </summary>
        public ProtoCategory CarryProtoCategory;

        /// <summary>
        /// 承运协议内容
        /// </summary>
        public byte[] Buffer;

        public CarryProto()
        {

        }

        public CarryProto(long accountId, ushort carryProtoCode, ProtoCategory carryProtoCategory, byte[] buffer)
        {
            AccountId = accountId;
            CarryProtoCode = carryProtoCode;
            CarryProtoCategory = carryProtoCategory;
            Buffer = buffer;
        }

        public byte[] ToArray(MMO_MemoryStream ms)
        {
            ms.SetLength(0);
            ms.WriteUShort(0);
            ms.WriteByte((byte)Category);
            ms.WriteLong(AccountId);
            ms.WriteUShort(CarryProtoCode);
            ms.WriteByte((byte)CarryProtoCategory);
            ms.WriteInt(Buffer.Length);
            ms.Write(Buffer);
            return ms.ToArray();
        }

        public static CarryProto GetProto(MMO_MemoryStream ms, byte[] buffer)
        {
            CarryProto proto = new CarryProto();
            ms.SetLength(0);
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;

            proto.AccountId = ms.ReadLong();
            proto.CarryProtoCode = ms.ReadUShort();
            proto.CarryProtoCategory = (ProtoCategory)ms.ReadByte();

            int len = ms.ReadInt();
            proto.Buffer = new byte[len];
            ms.Read(proto.Buffer);

            return proto;
        }
    }
}