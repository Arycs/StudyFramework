using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.YFMongoDB;

namespace YouYouServer.Core.Logger
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public  class LoggerEntity: YFMongoEntityBase
    {
        /// <summary>
        /// 日志等级
        /// </summary> 
        public LoggerLevel Level;

        /// <summary>
        /// 日志分类,比如某些系统的对应日志
        /// </summary>
        public int Category;

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message;
    }
}
