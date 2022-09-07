//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTMaterials数据管理
    /// </summary>
    public partial class DTMaterialsDBModel : DataTableDBModelBase<DTMaterialsDBModel, DTMaterialsEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTMaterials";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTMaterialsEntity entity = new DTMaterialsEntity();
                entity.Id = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();
                entity.Quality = ms.ReadInt();
                entity.Description = ms.ReadUTF8String();
                entity.Type = ms.ReadInt();
                entity.FixedType = ms.ReadInt();
                entity.FixedAddValue = ms.ReadInt();
                entity.maxAmount = ms.ReadInt();
                entity.packSort = ms.ReadInt();
                entity.CompositionProps = ms.ReadUTF8String();
                entity.CompositionMaterialID = ms.ReadInt();
                entity.CompositionGold = ms.ReadUTF8String();
                entity.SellMoney = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}