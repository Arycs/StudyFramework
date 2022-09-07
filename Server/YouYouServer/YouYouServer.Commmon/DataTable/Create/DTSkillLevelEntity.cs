//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSkillLevel实体
    /// </summary>
    public partial class DTSkillLevelEntity : DataTableEntityBase
    {
        /// <summary>
        /// 技能编号
        /// </summary>
        public int SkillId;

        /// <summary>
        /// 技能等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 技能预设编号
        /// </summary>
        public int PrefabId;

        /// <summary>
        /// 技能消耗的魔法值
        /// </summary>
        public int SpendMP;

        /// <summary>
        /// 此技能的CD间隔秒数
        /// </summary>
        public float SkillCDTime;

        /// <summary>
        /// 攻击范围
        /// </summary>
        public float AttackRange;

        /// <summary>
        /// 技能脚本编号
        /// </summary>
        public int ScriptId;

        /// <summary>
        /// 技能参数
        /// </summary>
        public int[] Args;

        /// <summary>
        /// 技能描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// 升级所需主角的等级
        /// </summary>
        public int NeedCharacterLevel;

        /// <summary>
        /// 升级消耗金币
        /// </summary>
        public int SpendGold;

    }
}