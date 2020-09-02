using GitHubStats.Context;
using GitHubStats.Models;
using GraphQL;
using GraphQL.Client.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace GitHubStats.Service
{
    public class GraphQLService : BackgroundService
    {
        private readonly ILogger<GraphQLService> _logger;
        private readonly GraphQLHttpClient _graphQLHttpClient;
        private readonly ICountryService _countryService;
        private readonly GitHubModel _gitHubModel;
        private GitHubModel.CountryModel countryModel;
        private int numberOfRequests = 1;
        private bool hasNextPage = true;
        private string endCursor = null;
        private GraphQLHttpResponse<GraphQLResponseModel> graphQLResponseModel;
        public GraphQLService(ILogger<GraphQLService> logger,
                              ICountryService countryService,
                              GraphQLModel graphModel,
                              GitHubModel gitHubModel)
        {
            _logger = logger;
            _countryService = countryService;
            _graphQLHttpClient = new GraphQLContext(graphModel).graphQLHttpClient;
            _gitHubModel = gitHubModel;
            countryModel = _gitHubModel.Country.First();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (hasNextPage)
                {
                    if (numberOfRequests <= _gitHubModel.MaxNumberOfRequests)
                    {
                        _logger.LogInformation("Country {country}", countryModel.Name);
                        try
                        {
                            graphQLResponseModel = await AddUser(new GraphQLRequestModel(countryModel, _gitHubModel.MaxRequestSize, endCursor));
                            hasNextPage = graphQLResponseModel.Data.search.pageInfo.hasNextPage;
                            endCursor = graphQLResponseModel.Data.search.pageInfo.endCursor;
                            numberOfRequests = numberOfRequests + 1;
                            _logger.LogInformation("Response {responseHeader}", graphQLResponseModel.ResponseHeaders.ToString());
                            _logger.LogInformation("Added Users hasNextPage {hasNextPage} endCursor {endCursor}", hasNextPage, endCursor);
                        }
                        catch (Exception e)
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
                    countryModel = GetCountryModel(countryModel.Name);
                    numberOfRequests = 0;
                    hasNextPage = true;
                    endCursor = null;
                }
                await Task.Delay(TimeSpan.FromSeconds(_gitHubModel.ElapsedTimeInSeconds), stoppingToken);
            }
        }
        private GitHubModel.CountryModel GetCountryModel(string countryName)
        {
            int countryIndex;
            int lastCountryIndex;
            int nextCountryIndex;

            countryIndex = _gitHubModel.Country.FindIndex(country => country.Name.Equals(countryName));
            lastCountryIndex = _gitHubModel.Country.Count() - 1;
            nextCountryIndex = countryIndex + 1;

            if ((countryIndex) < (lastCountryIndex))
            {
                return _gitHubModel.Country.ElementAt(nextCountryIndex++);
            } 
            else
            {
                return _gitHubModel.Country.ElementAt(0);
            } 
        }
        public async Task<GraphQLHttpResponse<GraphQLResponseModel>> AddUser(GraphQLRequestModel graphQLRequestModel)
        {
            var graphQLResponse = await GetGraphQLHttpResponse(graphQLRequestModel);
            {
                foreach (GraphQLResponseModel.SearchModel.EdgeModel userNode in graphQLResponse.Data.search.edges)
                {
                    if (!(userNode.node.login is null))
                    {
                        await _countryService.UpdateOneUserAsync(graphQLRequestModel.Country.Name, userNode.node);
                        _logger.LogInformation("Updated User {user} {countryName}", userNode.node.login, graphQLRequestModel.Country.Name);
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
                    search = graphQLRequestModel.Country.Search.First() + " sort:followers-desc " + LocationBuilder(graphQLRequestModel.Country.Search),
                    first = graphQLRequestModel.NumberOfUsers,
                    after = graphQLRequestModel.EndCursor
                }
            };
            return (GraphQLHttpResponse<GraphQLResponseModel>)await _graphQLHttpClient
                .SendQueryAsync<GraphQLResponseModel>(graphQLRequest);
        }

        private StringBuilder LocationBuilder(List<string> searchList)
        {
            StringBuilder locationList = new StringBuilder();
            foreach (string searchItem in searchList)
            {
                locationList.Append("location:" + searchItem + " ");
            }
            return locationList;
        }
    }
}