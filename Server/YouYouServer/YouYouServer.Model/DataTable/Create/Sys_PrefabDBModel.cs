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
    /// Sys_Prefab数据管理
    /// </summary>
    public partial class Sys_PrefabDBModel : DataTableDBModelBase<Sys_PrefabDBModel, Sys_PrefabEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/Sys_Prefab.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                Sys_PrefabEntity entity = new Sys_PrefabEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.AssetCategory = ms.ReadInt();
                entity.AssetPath = ms.ReadUTF8String();
                entity.PoolId = (byte)ms.ReadByte();
                entity.CullDespawned = (byte)ms.ReadByte();
                entity.CullAbove = ms.ReadInt();
                entity.CullDelay = ms.ReadInt();
                entity.CullMaxPerPass = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}