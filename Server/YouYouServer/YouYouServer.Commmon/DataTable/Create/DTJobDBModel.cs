//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTJob数据管理
    /// </summary>
    public partial class DTJobDBModel : DataTableDBModelBase<DTJobDBModel, DTJobEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTJob";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTJobEntity entity = new DTJobEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.Name = ms.ReadUTF8String();
                entity.BaseRoleId = ms.ReadInt();
                entity.HeadPic = ms.ReadUTF8String();
                entity.JobPic = ms.ReadUTF8String();
                entity.JobDesc = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}