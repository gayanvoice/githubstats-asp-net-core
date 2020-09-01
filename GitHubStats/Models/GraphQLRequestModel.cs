namespace GitHubStats.Models
{
    public class GraphQLRequestModel
    {
        public GraphQLRequestModel(GitHubModel.CountryModel country, int numberOfUsers, string endCursor)
        {
            Country = country;
            NumberOfUsers = numberOfUsers;
            EndCursor = endCursor;
        }
        public int NumberOfUsers { get; set; }
        public string EndCursor { get; set; }
        public GitHubModel.CountryModel Country { get; set; }
    }
}