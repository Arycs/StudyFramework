//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-05-30 21:08:38
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// Sys_StorySound数据管理
    /// </summary>
    public partial class Sys_StorySoundDBModel : DataTableDBModelBase<Sys_StorySoundDBModel, Sys_StorySoundEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/Sys_StorySound.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                Sys_StorySoundEntity entity = new Sys_StorySoundEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.AssetPath_CN = ms.ReadUTF8String();
                entity.AssetPath_EN = ms.ReadUTF8String();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}