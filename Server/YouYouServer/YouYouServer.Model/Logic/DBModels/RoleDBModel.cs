using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.YFMongoDB;
using YouYouServer.Model.Entitys;
using YouYouServer.Model.Managers;

namespace YouYouServer.Model.DBModels
{
    /// <summary>
    /// 角色的数据管理器
    /// </summary>
    public class RoleDBModel : YFMongoDBModelBase<RoleEntity>
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.GameServerDBName;

        protected override string CollectionName => "Role";
    }
}
