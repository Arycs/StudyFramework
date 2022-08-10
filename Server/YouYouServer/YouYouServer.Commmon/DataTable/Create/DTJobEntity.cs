//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTJob实体
    /// </summary>
    public partial class DTJobEntity : DataTableEntityBase
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// 关联角色Id
        /// </summary>
        public int RoleId;

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPic;

        /// <summary>
        /// 职业半身像
        /// </summary>
        public string JobPic;

        /// <summary>
        /// 职业描述
        /// </summary>
        public string JobDesc;

        /// <summary>
        /// 系数---攻击
        /// </summary>
        public int Attack;

        /// <summary>
        /// 系数--防御
        /// </summary>
        public int Defense;

        /// <summary>
        /// 系数--命中率
        /// </summary>
        public int Hit;

        /// <summary>
        /// 系数--闪避率
        /// </summary>
        public int Dodge;

        /// <summary>
        /// 系数--暴击率
        /// </summary>
        public int Cri;

        /// <summary>
        /// 系数--抗性
        /// </summary>
        public int Res;

    }
}