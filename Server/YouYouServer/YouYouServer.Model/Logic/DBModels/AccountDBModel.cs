using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YouYouServer.Core.YFMongoDB;
using YouYouServer.Model.Logic.Entitys;

namespace YouYouServer.Model.Logic.DBModels
{
    public class AccountDBModel : YFMongoDBModelBase<AccountEntity>
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.AccountDBName;

        protected override string CollectionName => "Account";
    }
}
