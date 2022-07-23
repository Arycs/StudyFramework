using System;

namespace YouYouServer.Common
{
    /// <summary>
    /// 数据表管理器
    /// 数据表的DBModel 写成静态属性放在此处,只可读,通过初始化来进行加载数据,供外部使用
    /// </summary>
    public sealed class DataTableManager
    {
        public static Sys_CodeDBModel Sys_CodeDBModel { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            //每个表都加载数据
            Sys_CodeDBModel = new Sys_CodeDBModel();
            Sys_CodeDBModel.LoadData();

            Console.WriteLine("LoadDataTable Complete");
        }
    }
}
