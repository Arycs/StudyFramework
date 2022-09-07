//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTShopCategory数据管理
    /// </summary>
    public partial class DTShopCategoryDBModel : DataTableDBModelBase<DTShopCategoryDBModel, DTShopCategoryEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTShopCategory";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTShopCategoryEntity entity = new DTShopCategoryEntity();
                entity.Id = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}