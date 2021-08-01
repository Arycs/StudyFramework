//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-07-19 00:08:33
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// Sys_SceneDetail数据管理
    /// </summary>
    public partial class Sys_SceneDetailDBModel : DataTableDBModelBase<Sys_SceneDetailDBModel, Sys_SceneDetailEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/Sys_SceneDetail.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                Sys_SceneDetailEntity entity = new Sys_SceneDetailEntity();
                entity.Id = ms.ReadInt();
                entity.SceneId = ms.ReadInt();
                entity.ScenePath = ms.ReadUTF8String();
                entity.SceneGrade = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}