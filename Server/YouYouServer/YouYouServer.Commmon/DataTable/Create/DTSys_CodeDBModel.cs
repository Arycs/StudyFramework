//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSys_Code数据管理
    /// </summary>
    public partial class DTSys_CodeDBModel : DataTableDBModelBase<DTSys_CodeDBModel, DTSys_CodeEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTSys_Code";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTSys_CodeEntity entity = new DTSys_CodeEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.Name = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}