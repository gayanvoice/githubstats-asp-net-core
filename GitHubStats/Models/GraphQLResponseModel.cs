using System.Collections.Generic;

namespace GitHubStats.Models
{
    public class GraphQLResponseModel
    {
        public Search search { get; set; }
        public class Search
        {
            public List<Edge> edges { get; set; }
            public PageInfo pageInfo { get; set; }
            public class Edge
            {
                public UserNode node { get; set; }
                public class UserNode
                {
                    public string login { get; set; }
                    public string name { get; set; }
                    public string avatarUrl { get; set; }
                    public string company { get; set; }
                    public string location { get; set; }
                    public Organizations organizations { get; set; }
                    public class Organizations
                    {
                        public List<OrganizationNode> nodes { get; set; }

                        public class OrganizationNode
                        {
                            public string login { get; set; }
                        }
                    }
                    public Followers followers { get; set; }
                    public class Followers
                    {
                        public int totalCount { get; set; }
                    }
                    public ContributionsCollection contributionsCollection { get; set; }
                    public class ContributionsCollection
                    {
                        public int restrictedContributionsCount { get; set; }
                        public ContributionCalendar contributionCalendar { get; set; }
                        public class ContributionCalendar
                        {
                            public int totalContributions { get; set; }
                        }
                    }
                }
            }
            public class PageInfo
            {
                public string endCursor { get; set; }
                public bool hasNextPage { get; set; }
            }
        }
    }
}