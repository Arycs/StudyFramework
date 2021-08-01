//===================================================
//作    者：边涯  http://www.u3dol.com
//创建时间：2021-07-19 00:08:32
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using YouYouServer.Core.DataTableBase;

namespace YouYouServer.Model.DataTable
{
    /// <summary>
    /// Sys_Audio实体
    /// </summary>
    public partial class Sys_AudioEntity : DataTableEntityBase
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// 路径
        /// </summary>
        public string AssetPath;

        /// <summary>
        /// 是否3D声音
        /// </summary>
        public int Is3D;

        /// <summary>
        /// 音量(0-1)
        /// </summary>
        public float volume;

    }
}