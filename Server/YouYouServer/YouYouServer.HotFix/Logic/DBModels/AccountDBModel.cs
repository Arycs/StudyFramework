using MongoDB.Driver;
using YouYouServer.Common;
using YouYouServer.Core;

namespace YouYouServer.Model
{
    public class AccountDBModel : YFMongoDBModelBase<AccountEntity>
    {
        protected override MongoClient Client => MongoDBClient.CurrClient;

        protected override string DatabaseName => ServerConfig.AccountDBName;

        protected override string CollectionName => "Account";
    }
}
