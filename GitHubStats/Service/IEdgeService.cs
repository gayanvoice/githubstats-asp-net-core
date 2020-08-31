using GitHubStats.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubStats.Service
{
    public interface IEdgeService
    {
        Task<GraphQLResponseModel.Search.Edge.UserNode> CreateCountryAsync (GraphQLResponseModel.Search.Edge.UserNode edgeModel);
        Task<GraphQLResponseModel.Search.Edge.UserNode> UpdateCountryAsync(GraphQLResponseModel.Search.Edge.UserNode edgeModel);
        List<GraphQLResponseModel.Search.Edge.UserNode> GetList();
    }
}