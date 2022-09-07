//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTWorldMap数据管理
    /// </summary>
    public partial class DTWorldMapDBModel : DataTableDBModelBase<DTWorldMapDBModel, DTWorldMapEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTWorldMap";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTWorldMapEntity entity = new DTWorldMapEntity();
                entity.Id = ms.ReadInt();
                entity.Name = ms.ReadUTF8String();
                entity.SceneName = ms.ReadUTF8String();
                entity.SmallMapImg = ms.ReadUTF8String();
                entity.NPCList = ms.ReadUTF8String();
                entity.RoleBirthPos = ms.ReadUTF8String();
                entity.CameraRotation = ms.ReadUTF8String();
                entity.TransPos = ms.ReadUTF8String();
                entity.IsCity = ms.ReadInt();
                entity.IsShowInMap = ms.ReadInt();
                entity.PosInMap = ms.ReadUTF8String();
                entity.IcoInMap = ms.ReadUTF8String();
                entity.NearScene = ms.ReadUTF8String();
                entity.Audio_BG = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}