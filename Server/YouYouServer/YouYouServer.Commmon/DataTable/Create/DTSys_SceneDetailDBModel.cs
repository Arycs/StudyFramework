//===================================================
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTSys_SceneDetail数据管理
    /// </summary>
    public partial class DTSys_SceneDetailDBModel : DataTableDBModelBase<DTSys_SceneDetailDBModel, DTSys_SceneDetailEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableName => "DTSys_SceneDetail";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTSys_SceneDetailEntity entity = new DTSys_SceneDetailEntity();
                entity.Id = ms.ReadInt();
                entity.SceneId = ms.ReadInt();
                entity.SceneName = ms.ReadUTF8String();
                entity.ScenePath = ms.ReadUTF8String();
                entity.SceneGrade = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}