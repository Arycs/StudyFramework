//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSkillLevel数据管理
    /// </summary>
    public partial class DTSkillLevelDBModel : DataTableDBModelBase<DTSkillLevelDBModel, DTSkillLevelEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTSkillLevel";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTSkillLevelEntity entity = new DTSkillLevelEntity();
                entity.Id = ms.ReadInt();
                entity.SkillId = ms.ReadInt();
                entity.Level = ms.ReadInt();
                entity.PrefabId = ms.ReadInt();
                entity.SpendMP = ms.ReadInt();
                entity.SkillCDTime = ms.ReadFloat();
                entity.AttackRange = ms.ReadFloat();
                entity.ScriptId = ms.ReadInt();
                entity.Args = ms.ReadIntArr();
                entity.Desc = ms.ReadUTF8String();
                entity.NeedCharacterLevel = ms.ReadInt();
                entity.SpendGold = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}