using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GitHubStats.Models
{
    public class UserBsonModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("login")]
        public string Login { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("avatarUrl")]
        public string AvatarUrl { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("contributionsCollection")]
        public ContributionsCollectionModel ContributionsCollection { get; set; }

        [BsonIgnoreExtraElements]
        public class ContributionsCollectionModel
        {
            [BsonElement("restrictedContributionsCount")]
            public int RestrictedContirbutionsCount { get; set; }

            [BsonElement("contributionCalendar")]
            public ContributionCalendarModel ContributionCalendar { get; set; }

            [BsonIgnoreExtraElements]
            public class ContributionCalendarModel
            {
                [BsonElement("totalContributions")]
                public int TotalContributions { get; set; }


            }

        }

        [BsonElement("followers")]
        public FollowersModel Followers { get; set; }

        [BsonIgnoreExtraElements]
        public class FollowersModel
        {
            [BsonElement("totalCount")]
            public int TotalCount { get; set; }
        }

        [BsonElement("organizations")]
        public OrganizationsModel Organizations { get; set; }

        [BsonIgnoreExtraElements]
        public class OrganizationsModel
        {
            [BsonElement("nodes")]
            public List<OrganizationsNodesModel> Nodes { get; set; }

            [BsonIgnoreExtraElements]
            public class OrganizationsNodesModel
            {
                [BsonElement("login")]
                public string Login { get; set; }
            }
        }
    }
}