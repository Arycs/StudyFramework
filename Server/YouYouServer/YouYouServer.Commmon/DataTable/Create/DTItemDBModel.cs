//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTItem数据管理
    /// </summary>
    public partial class DTItemDBModel : DataTableDBModelBase<DTItemDBModel, DTItemEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTItem";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTItemEntity entity = new DTItemEntity();
                entity.Id = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();
                entity.Type = ms.ReadInt();
                entity.UsedLevel = ms.ReadInt();
                entity.UsedMethod = ms.ReadUTF8String();
                entity.SellMoney = ms.ReadInt();
                entity.Quality = ms.ReadInt();
                entity.Description = ms.ReadUTF8String();
                entity.UsedItems = ms.ReadUTF8String();
                entity.maxAmount = ms.ReadInt();
                entity.packSort = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}