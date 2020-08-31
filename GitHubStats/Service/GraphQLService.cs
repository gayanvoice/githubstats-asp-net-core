using GitHubStats.Context;
using GitHubStats.Models;
using GraphQL;
using GraphQL.Client.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace GitHubStats.Service
{
    public class GraphQLService : BackgroundService
    {
        private readonly ILogger<GraphQLService> _logger;
        private readonly GraphQLHttpClient _graphQLHttpClient;
        private readonly IEdgeService _edgeService;
        private readonly GitHubModel _gitHubModel;
        private string countryName;
        private int numberOfRequests = 1;
        private bool hasNextPage = true;
        private string endCursor = null;
        private GraphQLHttpResponse<GraphQLResponseModel> graphQLResponseModel;
        public GraphQLService(ILogger<GraphQLService> logger,
                              IEdgeService edgeService,
                              GraphQLModel graphModel,
                              GitHubModel gitHubModel)
        {
            _logger = logger;
            _edgeService = edgeService;
            _graphQLHttpClient = new GraphQLContext(graphModel).graphQLHttpClient;
            _gitHubModel = gitHubModel;
            countryName = _gitHubModel.Country.First();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (hasNextPage)
                { 
                    if (numberOfRequests <= _gitHubModel.MaxNumberOfRequests)
                    {
                        _logger.LogInformation("Country {country}", countryName);
                        try
                        {
                            graphQLResponseModel = await AddUser(new GraphQLRequestModel(countryName, _gitHubModel.MaxRequestSize, endCursor));
                            hasNextPage = graphQLResponseModel.Data.search.pageInfo.hasNextPage;
                            endCursor = graphQLResponseModel.Data.search.pageInfo.endCursor;
                            numberOfRequests  = numberOfRequests + 1;
                            _logger.LogInformation("Response {responseHeader}", graphQLResponseModel.ResponseHeaders.ToString());
                            _logger.LogInformation("Added Users hasNextPage {hasNextPage} endCursor {endCursor}", hasNextPage, endCursor);
                        }
                        catch(Exception e)
                        {
                            hasNextPage = false;
                            _logger.LogInformation("Add Exception Some GraphQL Server Error {Exception}", e);
                        }
                    }
                    else
                    {
                        hasNextPage = false;
                        _logger.LogInformation("Max Cap NumberOfUsers");
                    }
                }
                else
                {
                    _logger.LogInformation("No hasNextPage");
                    countryName = GetCountry(countryName);
                    numberOfRequests = 0;
                    hasNextPage = true;
                    endCursor = null;
                }
                await Task.Delay(TimeSpan.FromSeconds(_gitHubModel.ElapsedTimeInSeconds), stoppingToken);
            }
        }
        private string GetCountry(string countryName)
        {
            int countryIndex;
            int lastIndex;
            int nextCountryIndex;
            if (string.IsNullOrEmpty(countryName))
            {
                return _gitHubModel.Country.ElementAt(0);
            }
            else
            {
                if (_gitHubModel.Country.Contains(countryName))
                {
                    countryIndex = _gitHubModel.Country.FindIndex(country => country.Equals(countryName));
                    lastIndex = _gitHubModel.Country.Count() - 1;
                    nextCountryIndex = countryIndex + 1;

                    if ((countryIndex) < (lastIndex))
                    {
                        return _gitHubModel.Country.ElementAt(nextCountryIndex++);
                    } 
                    else
                    {
                        return _gitHubModel.Country.ElementAt(0);
                    } 
                }
                else
                {
                    return _gitHubModel.Country.ElementAt(0);
                }
            }
        }
        public async Task<GraphQLHttpResponse<GraphQLResponseModel>> AddUser(GraphQLRequestModel graphQLRequestModel)
        {
            var graphQLResponse = await GetGraphQLHttpResponse(graphQLRequestModel);
            {
                foreach (GraphQLResponseModel.Search.Edge userNode in graphQLResponse.Data.search.edges)
                {
                    if (!(userNode.node.login is null))
                    {
                        await _edgeService.UpdateCountryAsync(userNode.node);
                        _logger.LogInformation("Added User {user}", userNode.node.login);
                    }
                }  
            }
            return graphQLResponse;
        }
            public async Task<GraphQLHttpResponse<GraphQLResponseModel>> GetGraphQLHttpResponse(GraphQLRequestModel graphQLRequestModel)
        {
            var graphQLRequest = new GraphQLRequest
            {
                Query = @"query ($search: String!, $first: Int!, $after: String) {
                            search(type: USER, query: $search, first: $first, after: $after) {
                                edges {
                                    node {
                                        ... on User {
                                            login
                                            name
                                            avatarUrl(size: 72)
                                            company
                                            location
                                            organizations(first: 10) {
                                                nodes {
                                                    login
                                                }
                                            }
                                            followers {
                                                totalCount
                                            }
                                            contributionsCollection {
                                                contributionCalendar {
                                                    totalContributions
                                                }
                                            restrictedContributionsCount
                                            }                                                                
                                        }
                                    }
                                }
                                pageInfo {
                                    endCursor
                                    hasNextPage
                                }
                            }
                        }",
                Variables = new
                {
                    search = graphQLRequestModel.Country + " sort:followers-desc  location:" + graphQLRequestModel.Country,
                    first = graphQLRequestModel.NumberOfUsers,
                    after = graphQLRequestModel.EndCursor
                }
            };
            return (GraphQLHttpResponse<GraphQLResponseModel>)await _graphQLHttpClient
                .SendQueryAsync<GraphQLResponseModel>(graphQLRequest);
        }
    }
}