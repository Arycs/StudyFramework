//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTGameLevelMonster数据管理
    /// </summary>
    public partial class DTGameLevelMonsterDBModel : DataTableDBModelBase<DTGameLevelMonsterDBModel, DTGameLevelMonsterEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTGameLevelMonster";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTGameLevelMonsterEntity entity = new DTGameLevelMonsterEntity();
                entity.Id = ms.ReadInt();
                entity.GameLevelId = ms.ReadInt();
                entity.Grade = ms.ReadInt();
                entity.RegionId = ms.ReadInt();
                entity.SpriteId = ms.ReadInt();
                entity.SpriteCount = ms.ReadInt();
                entity.Exp = ms.ReadInt();
                entity.Gold = ms.ReadInt();
                entity.DropEquip = ms.ReadUTF8String();
                entity.DropItem = ms.ReadUTF8String();
                entity.DropMaterial = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}