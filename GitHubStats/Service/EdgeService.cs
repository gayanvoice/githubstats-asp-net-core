using GitHubStats.Context;
using GitHubStats.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubStats.Service
{
    public class EdgeService : IEdgeService
    {
        private IMongoCollection<GraphQLResponseModel.Search.Edge.UserNode> collection;

        public EdgeService(MongoModel mongoModel)
        {
            collection = new MongoContext(mongoModel)
                .database
                .GetCollection<GraphQLResponseModel.Search.Edge.UserNode>(mongoModel.Collection.User);
        }
        public List<GraphQLResponseModel.Search.Edge.UserNode> GetList()
        {
            return collection.Find(user => true).ToList();
        }

        //public GraphQLResponseModel.Search.Edge.UserNode GetCountry(string countryName)
        //{
        //    return collection.Find<CountryModel>(country => country.CountryName == countryName).FirstOrDefault();
        //}

        public async Task<GraphQLResponseModel.Search.Edge.UserNode> CreateCountryAsync(GraphQLResponseModel.Search.Edge.UserNode edgeModel)
        {
            await collection.InsertOneAsync(edgeModel);
            return edgeModel;
        }

        public async Task<GraphQLResponseModel.Search.Edge.UserNode> UpdateCountryAsync(GraphQLResponseModel.Search.Edge.UserNode edgeModel)
        {
            await collection.UpdateOneAsync(new BsonDocument {{"login", edgeModel.login}},
                                            new BsonDocument {{"$set", edgeModel.ToBsonDocument()}},
                                            new UpdateOptions{IsUpsert = true});
            return edgeModel;
        }
    }
}