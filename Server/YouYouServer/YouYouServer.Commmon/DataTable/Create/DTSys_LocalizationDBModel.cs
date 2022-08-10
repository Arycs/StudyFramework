//===================================================
//作    者：边涯  http://www.u3dol.com
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSys_Localization数据管理
    /// </summary>
    public partial class DTSys_LocalizationDBModel : DataTableDBModelBase<DTSys_LocalizationDBModel, DTSys_LocalizationEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/DTSys_Localization.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTSys_LocalizationEntity entity = new DTSys_LocalizationEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.Key = ms.ReadUTF8String();
                entity.Chinese = ms.ReadUTF8String();
                entity.English = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}