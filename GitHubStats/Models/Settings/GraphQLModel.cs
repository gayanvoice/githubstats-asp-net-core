using System.Text.Json.Serialization;

namespace GitHubStats.Models
{
    public class GraphQLModel
    {
        [JsonPropertyName("connection")]
        public string Connection { get; set; }
        
        [JsonPropertyName("authentication")]
        public string Authentication { get; set; }
    }
}