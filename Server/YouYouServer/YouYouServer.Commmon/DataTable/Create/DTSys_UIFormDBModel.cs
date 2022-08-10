//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSys_UIForm数据管理
    /// </summary>
    public partial class DTSys_UIFormDBModel : DataTableDBModelBase<DTSys_UIFormDBModel, DTSys_UIFormEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTSys_UIForm";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTSys_UIFormEntity entity = new DTSys_UIFormEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.Name = ms.ReadUTF8String();
                entity.UIGroupId = (byte)ms.ReadByte();
                entity.DisableUILayer = ms.ReadInt();
                entity.IsLock = ms.ReadInt();
                entity.AssetPath_Chinese = ms.ReadUTF8String();
                entity.AssetPath_English = ms.ReadUTF8String();
                entity.CanMulit = ms.ReadBool();
                entity.ShowMode = (byte)ms.ReadByte();
                entity.FreezeMode = (byte)ms.ReadByte();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}