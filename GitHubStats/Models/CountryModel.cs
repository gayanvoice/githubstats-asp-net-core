using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GitHubStats.Models
{
    public class CountryModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("_country")]
        public string CountryId { get; set; }

        [BsonElement("country")]
        public string CountryName { get; set; }

        [BsonElement("dataset")]
        public List<UserObject> DataSet { get; set; }

        [BsonElement("modified")]
        public DateTime Modified { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserObject
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("avatar_url")]
        public string AvatarUrl { get; set; }

        [BsonElement("login")]
        public string Login { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("followers")]
        public int Followers { get; set; }

        [BsonElement("public_contributions")]
        public int PublicContributions { get; set; }

        [BsonElement("private_contributions")]
        public int PrivateContributions { get; set; }
    }
}