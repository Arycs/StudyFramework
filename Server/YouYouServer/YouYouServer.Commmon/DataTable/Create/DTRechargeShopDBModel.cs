//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTRechargeShop数据管理
    /// </summary>
    public partial class DTRechargeShopDBModel : DataTableDBModelBase<DTRechargeShopDBModel, DTRechargeShopEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTRechargeShop";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTRechargeShopEntity entity = new DTRechargeShopEntity();
                entity.Id = ms.ReadInt();
                entity.Type = ms.ReadInt();
                entity.Price = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();
                entity.SalesDesc = ms.ReadUTF8String();
                entity.ProductDesc = ms.ReadUTF8String();
                entity.Virtual = ms.ReadInt();
                entity.Icon = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}