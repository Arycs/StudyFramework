//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTShop数据管理
    /// </summary>
    public partial class DTShopDBModel : DataTableDBModelBase<DTShopDBModel, DTShopEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTShop";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTShopEntity entity = new DTShopEntity();
                entity.Id = ms.ReadInt();
                entity.ShopCategoryId = ms.ReadInt();
                entity.GoodsType = ms.ReadInt();
                entity.GoodsId = ms.ReadInt();
                entity.OldPrice = ms.ReadInt();
                entity.Price = ms.ReadInt();
                entity.SellStatus = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}