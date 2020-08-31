using GitHubStats.Models;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace GitHubStats.Context
{
    public class GraphQLContext
    {
        public GraphQLContext(GraphQLModel graphModel)
        {
            graphQLHttpClient = new GraphQLHttpClient(graphModel.Connection, new NewtonsoftJsonSerializer());
            graphQLHttpClient
                .HttpClient
                .DefaultRequestHeaders
                .Add("Authorization", graphModel.Authentication);
        }
        public GraphQLHttpClient graphQLHttpClient { get; }
    }
}