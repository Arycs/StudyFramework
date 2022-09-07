//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTGameLevelRegion数据管理
    /// </summary>
    public partial class DTGameLevelRegionDBModel : DataTableDBModelBase<DTGameLevelRegionDBModel, DTGameLevelRegionEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTGameLevelRegion";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTGameLevelRegionEntity entity = new DTGameLevelRegionEntity();
                entity.Id = ms.ReadInt();
                entity.GameLevelId = ms.ReadInt();
                entity.RegionId = ms.ReadInt();
                entity.InitSprite = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}