//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-06-14 17:37:16
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// Sys_Audio数据管理
    /// </summary>
    public partial class Sys_AudioDBModel : DataTableDBModelBase<Sys_AudioDBModel, Sys_AudioEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/Sys_Audio.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                Sys_AudioEntity entity = new Sys_AudioEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.AssetPath = ms.ReadUTF8String();
                entity.Is3D = ms.ReadInt();
                entity.volume = ms.ReadFloat();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}