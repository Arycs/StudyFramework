using YouYouServer.Core;

namespace YouYouServer.Common.DBData
{
    /// <summary>
    /// 角色的简要信息, 只是在客户端选择角色UI的界面显示所需数据
    /// </summary>
    public class RoleBriefEntity :YFMongoEntityBase
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        public long RoleId;

        /// <summary>
        /// 职业
        /// </summary>
        public byte JobId;

        /// <summary>
        /// 性别
        /// </summary>
        public byte Sex;

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;
    }
}