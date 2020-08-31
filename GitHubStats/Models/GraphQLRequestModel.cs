namespace GitHubStats.Models
{
    public class GraphQLRequestModel
    {
        public GraphQLRequestModel(string country, int numberOfUsers, string endCursor)
        {
            Country = country;
            NumberOfUsers = numberOfUsers;
            EndCursor = endCursor;
        }
        public string Country { get; set; }
        public int NumberOfUsers { get; set; }
        public string EndCursor { get; set; }
    }
}