//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTRoleAnimCategory数据管理
    /// </summary>
    public partial class DTRoleAnimCategoryDBModel : DataTableDBModelBase<DTRoleAnimCategoryDBModel, DTRoleAnimCategoryEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTRoleAnimCategory";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTRoleAnimCategoryEntity entity = new DTRoleAnimCategoryEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.IdleNormalAnimId = ms.ReadInt();
                entity.RunAnimId = ms.ReadInt();
                entity.Attack_1 = ms.ReadInt();
                entity.Attack_2 = ms.ReadInt();
                entity.Attack_3 = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}