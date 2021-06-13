using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.YFMongoDB;
using YouYouServer.Model.Managers;

namespace YouYouServer.Model.Logic.DBModels
{
    public class UniqueIDGameServer : YFUniqueIDBase
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.GameServerDBName;

        protected override string CollectionName => "UniqueIDGameServer";
    }
}
