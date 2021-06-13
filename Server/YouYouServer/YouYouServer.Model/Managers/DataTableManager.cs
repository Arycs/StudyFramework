using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Model.DataTable;

namespace YouYouServer.Model.Managers
{
    /// <summary>
    /// 数据表管理器
    /// 数据表的DBModel 写成静态属性放在次数,只可读,通过初始化来进行加载数据,供外部使用
    /// </summary>
    public sealed class DataTableManager
    {
        /// <summary>
        /// UI表
        /// </summary>
        public static Sys_UIFormDBModel Sys_UIFormDBModel
        {
            get;private set;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            //实例化表, 并加载数据
            Sys_UIFormDBModel = new Sys_UIFormDBModel();
            Sys_UIFormDBModel.LoadData();

            Console.WriteLine("LoadDataTable Complete");
        }
    }
}
