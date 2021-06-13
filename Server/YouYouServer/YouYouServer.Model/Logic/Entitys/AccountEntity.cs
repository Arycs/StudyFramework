using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.YFMongoDB;

namespace YouYouServer.Model.Logic.Entitys
{
    /// <summary>
    /// 账号实体
    /// </summary>
    public class AccountEntity : YFMongoEntityBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password;
    }
}
