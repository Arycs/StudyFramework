//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-05-30 21:08:38
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// Sys_UIForm实体
    /// </summary>
    public partial class Sys_UIFormEntity : DataTableEntityBase
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// UI分组编号
        /// </summary>
        public byte UIGroupId;

        /// <summary>
        /// 禁用层级管理
        /// </summary>
        public int DisableUILayer;

        /// <summary>
        /// 是否锁定
        /// </summary>
        public int IsLock;

        /// <summary>
        /// 路径
        /// </summary>
        public string AssetPath_Chinese;

        /// <summary>
        /// 路径
        /// </summary>
        public string AssetPath_English;

    }
}