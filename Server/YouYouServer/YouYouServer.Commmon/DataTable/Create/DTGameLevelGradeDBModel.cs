//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTGameLevelGrade数据管理
    /// </summary>
    public partial class DTGameLevelGradeDBModel : DataTableDBModelBase<DTGameLevelGradeDBModel, DTGameLevelGradeEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTGameLevelGrade";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTGameLevelGradeEntity entity = new DTGameLevelGradeEntity();
                entity.Id = ms.ReadInt();
                entity.GameLevelId = ms.ReadInt();
                entity.Grade = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.Type = ms.ReadInt();
                entity.Parameter = ms.ReadUTF8String();
                entity.ConditionDesc = ms.ReadUTF8String();
                entity.Exp = ms.ReadInt();
                entity.Gold = ms.ReadInt();
                entity.CommendFighting = ms.ReadInt();
                entity.TimeLimit = ms.ReadFloat();
                entity.Star1 = ms.ReadFloat();
                entity.Star2 = ms.ReadFloat();
                entity.Equip = ms.ReadUTF8String();
                entity.Item = ms.ReadUTF8String();
                entity.Material = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}