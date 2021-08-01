//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-07-19 00:08:32
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// Sys_Effect数据管理
    /// </summary>
    public partial class Sys_EffectDBModel : DataTableDBModelBase<Sys_EffectDBModel, Sys_EffectEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/Sys_Effect.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                Sys_EffectEntity entity = new Sys_EffectEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.PrefabId = ms.ReadInt();
                entity.KeepTime = ms.ReadFloat();
                entity.SoundId = ms.ReadInt();
                entity.Type = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}