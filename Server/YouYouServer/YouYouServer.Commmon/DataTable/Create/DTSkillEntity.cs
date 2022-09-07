//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSkill实体
    /// </summary>
    public partial class DTSkillEntity : DataTableEntityBase
    {
        /// <summary>
        /// 技能名称
        /// </summary>
        public string SkillName;

        /// <summary>
        /// 技能描述
        /// </summary>
        public string SkillDesc;

        /// <summary>
        /// 技能释放按钮图片
        /// </summary>
        public string SkillPic;

        /// <summary>
        /// 技能最大等级
        /// </summary>
        public int LevelLimit;

        /// <summary>
        /// 是否被动技能
        /// </summary>
        public int IsPassive;

    }
}