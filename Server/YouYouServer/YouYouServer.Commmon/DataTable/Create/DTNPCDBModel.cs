//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTNPC数据管理
    /// </summary>
    public partial class DTNPCDBModel : DataTableDBModelBase<DTNPCDBModel, DTNPCEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTNPC";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTNPCEntity entity = new DTNPCEntity();
                entity.Id = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();
                entity.PrefabName = ms.ReadUTF8String();
                entity.HeadPic = ms.ReadUTF8String();
                entity.HalfBodyPic = ms.ReadUTF8String();
                entity.Talk = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}