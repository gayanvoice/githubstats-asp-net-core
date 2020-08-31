using GitHubStats.Models;
using MongoDB.Driver;

namespace GitHubStats.Context
{
    public class MongoContext
    {
        public MongoContext(MongoModel mongoModel)
        {
            var client = new MongoClient(mongoModel.Connection);
            database = client.GetDatabase(mongoModel.Database);
        }
        public IMongoDatabase database { get; }
    }
}