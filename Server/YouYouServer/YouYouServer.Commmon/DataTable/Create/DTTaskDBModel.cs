//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTTask数据管理
    /// </summary>
    public partial class DTTaskDBModel : DataTableDBModelBase<DTTaskDBModel, DTTaskEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTTask";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTTaskEntity entity = new DTTaskEntity();
                entity.Id = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();
                entity.Status = ms.ReadInt();
                entity.Content = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}