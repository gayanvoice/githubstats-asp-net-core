namespace GitHubStats.Models
{
    public class FindUserRequestModel
    {
        public string CountryName { get; set; }
        public int Limit { get; set; }
        public int Skip { get; set; }
    }
}