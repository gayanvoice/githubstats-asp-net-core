using GitHubStats.Context;
using GitHubStats.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GitHubStats.Models.GraphQLResponseModel.SearchModel.EdgeModel;

namespace GitHubStats.Service
{
    public class CountryService : ICountryService
    {
        private IMongoCollection<UserBsonModel> collection;
       
        public CountryService(MongoModel mongoModel)
        {
            collection = new MongoContext(mongoModel)
                .database
                .GetCollection<UserBsonModel>(mongoModel.Collection.User);
        }
        public List<UserBsonModel> GetUserListByCountry(FindUserRequestModel findUserRequestModel)
        {
            var filter = Builders<UserBsonModel>.Filter.Eq("country", findUserRequestModel.CountryName);
            return collection
                .Find<UserBsonModel>(filter)        
                .SortByDescending(user => user.ContributionsCollection.ContributionCalendar.TotalContributions)
                .Limit(findUserRequestModel.Limit)
                .Skip(findUserRequestModel.Skip)
                .ToList();
        }
        public async Task UpdateOneUserAsync(string countryName, UserNodeModel userNodeModel)
        {

            BsonDocument bsonDocument = new BsonDocument();
            bsonDocument.Add("$set", userNodeModel.ToBsonDocument().Add("country", countryName));

            await collection.UpdateOneAsync(new BsonDocument { { "login", userNodeModel.login}},
                                            bsonDocument,
                                            new UpdateOptions{ IsUpsert = true });
        }
    }
}