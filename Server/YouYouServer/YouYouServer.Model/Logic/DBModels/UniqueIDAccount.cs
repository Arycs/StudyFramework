using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.YFMongoDB;

namespace YouYouServer.Model.Logic.DBModels
{
    public class UniqueIDAccount : YFUniqueIDBase
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.AccountDBName;

        protected override string CollectionName => "UniqueIDAccount";
    }
}
