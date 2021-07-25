//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2020-02-18 23:03:22
//备    注：
//===================================================

using YouYouServer.Core;

namespace YouYou
{
    /// <summary>
    /// 协议接口
    /// </summary>
    public interface IProto
    {
        /// <summary>
        /// 协议编码
        /// </summary>
        string ProtoEnName { get; }

        /// <summary>
        /// 协议分类
        /// </summary>
        ProtoCategory Category { get; }
    }
}