using GitHubStats.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubStats.Service
{
    public interface ICountryService
    {
        Task UpdateOneUserAsync(string countryName, GraphQLResponseModel.SearchModel.EdgeModel.UserNodeModel edgeModel);
        List<UserBsonModel> GetUserListByCountry(FindUserRequestModel findUserRequestModel);
    }
}