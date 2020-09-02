namespace GitHubStats.Models
{
    public class MongoModel
    {
        public string Connection { get; set; }
        public string Database { get; set; }

        public CollectionModel Collection { get; set; }

        public class CollectionModel
        {
            public string User { get; set; }
            public string Country { get; set; }
        }
    }
}