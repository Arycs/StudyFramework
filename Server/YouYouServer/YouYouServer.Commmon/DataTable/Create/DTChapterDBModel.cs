//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTChapter数据管理
    /// </summary>
    public partial class DTChapterDBModel : DataTableDBModelBase<DTChapterDBModel, DTChapterEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTChapter";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTChapterEntity entity = new DTChapterEntity();
                entity.Id = ms.ReadInt();
                entity.ChapterName = ms.ReadUTF8String();
                entity.GameLevelCount = ms.ReadInt();
                entity.BG_Pic = ms.ReadUTF8String();
                entity.BranchLevelId_1 = ms.ReadInt();
                entity.BranchLevelName_1 = ms.ReadUTF8String();
                entity.BranchLevelId_2 = ms.ReadInt();
                entity.BranchLevelName_2 = ms.ReadUTF8String();
                entity.Uvx = ms.ReadFloat();
                entity.Uvy = ms.ReadFloat();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}