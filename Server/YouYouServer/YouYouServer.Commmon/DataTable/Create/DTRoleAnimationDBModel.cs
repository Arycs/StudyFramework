//===================================================
//作    者：边涯  http://www.u3dol.com
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;
using YouYouServer.Core.Common;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// DTRoleAnimation数据管理
    /// </summary>
    public partial class DTRoleAnimationDBModel : DataTableDBModelBase<DTRoleAnimationDBModel, DTRoleAnimationEntity>
    {
        /// <summary>
        /// 数据表完整路径
        /// </summary>
        public override string DataTableFullPath => ServerConfig.DataTablePath + "/DTRoleAnimation.bytes";

        /// <summary>
        /// 加载列表
        /// </summary>
        protected override void LoadList(MMO_MemoryStream ms)
        {
            int rows = ms.ReadInt();
            int columns = ms.ReadInt();

            for (int i = 0; i < rows; i++)
            {
                DTRoleAnimationEntity entity = new DTRoleAnimationEntity();
                entity.Id = ms.ReadInt();
                entity.Desc = ms.ReadUTF8String();
                entity.GroupId = ms.ReadInt();
                entity.AnimPath = ms.ReadUTF8String();
                entity.InitLoad = (byte)ms.ReadByte();
                entity.Expire = ms.ReadInt();

                m_List.Add(entity);
                m_Dic[entity.Id] = entity;
            }
        }
    }
}