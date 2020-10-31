using GitHubStats.Models;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace GitHubStats.Context
{
    public class GraphQLContext
    {
        public GraphQLContext(GraphQLModel graphQLModel)
        {
            graphQLHttpClient = new GraphQLHttpClient(graphQLModel.Connection, new NewtonsoftJsonSerializer());
            graphQLHttpClient
                .HttpClient
                .DefaultRequestHeaders
                .Add("Authorization", graphQLModel.Authentication);
        }
        public GraphQLHttpClient graphQLHttpClient { get; }
    }
}