//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTBuff数据管理
    /// </summary>
    public partial class DTBuffDBModel : DataTableDBModelBase<DTBuffDBModel, DTBuffEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTBuff";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTBuffEntity entity = new DTBuffEntity();
                entity.Id = ms.ReadInt();
                entity.ScriptId = ms.ReadInt();
                entity.IsControl = ms.ReadInt();
                entity.IsGran = ms.ReadInt();
                entity.CanDispel = ms.ReadInt();
                entity.BuffName = ms.ReadUTF8String();
                entity.PrefabId = ms.ReadInt();
                entity.Position = ms.ReadInt();
                entity.IsAnimation = ms.ReadInt();
                entity.KeepType = ms.ReadInt();
                entity.KeepTime = ms.ReadFloat();
                entity.BuffIcon = ms.ReadUTF8String();
                entity.BuffDesc = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}