//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTJobLevel实体
    /// </summary>
    public partial class DTJobLevelEntity : DataTableEntityBase
    {
        /// <summary>
        /// 职业编号
        /// </summary>
        public int JobId;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;

        /// <summary>
        /// 从本级升到下一级所需经验
        /// </summary>
        public int NeedExp;

        /// <summary>
        /// 体力
        /// </summary>
        public int Energy;

        /// <summary>
        /// 基础血量
        /// </summary>
        public int HP;

        /// <summary>
        /// 基础魔法值
        /// </summary>
        public int MP;

        /// <summary>
        /// 攻击
        /// </summary>
        public int atk;

        /// <summary>
        /// 防御
        /// </summary>
        public int def;

        /// <summary>
        /// 暴击率
        /// </summary>
        public int criticalRate;

        /// <summary>
        /// 抗暴率
        /// </summary>
        public int criticalResRate;

        /// <summary>
        /// 暴击强度率
        /// </summary>
        public int criticalStrengthRate;

        /// <summary>
        /// 格档率
        /// </summary>
        public int blockRate;

        /// <summary>
        /// 破击率
        /// </summary>
        public int blockResRate;

        /// <summary>
        /// 格挡强度率
        /// </summary>
        public int blockStrengthRate;

        /// <summary>
        /// 增伤率
        /// </summary>
        public int injureRate;

        /// <summary>
        /// 减伤率
        /// </summary>
        public int injureResRate;

        /// <summary>
        /// 技能伤害率
        /// </summary>
        public int eXSkillInjureRate;

        /// <summary>
        /// 技能抗性率
        /// </summary>
        public int eXSkillInjureResRate;

        /// <summary>
        /// 无视防御率
        /// </summary>
        public int IgnoreDefRate;

    }
}