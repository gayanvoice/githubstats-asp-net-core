using System.Collections.Generic;

namespace GitHubStats.Models
{
    public class GraphQLResponseModel
    {
        public SearchModel search { get; set; }
        public class SearchModel
        {
            public List<EdgeModel> edges { get; set; }
            public PageInfoModel pageInfo { get; set; }
            public class EdgeModel
            {
                public UserNodeModel node { get; set; }
                public class UserNodeModel
                {
                    public string login { get; set; }
                    public string name { get; set; }
                    public string avatarUrl { get; set; }
                    public string company { get; set; }
                    public string location { get; set; }
                    public OrganizationsModel organizations { get; set; }
                    public class OrganizationsModel
                    {
                        public List<OrganizationNodeModel> nodes { get; set; }

                        public class OrganizationNodeModel
                        {
                            public string login { get; set; }
                        }
                    }
                    public FollowersModel followers { get; set; }
                    public class FollowersModel
                    {
                        public int totalCount { get; set; }
                    }
                    public ContributionsCollectionModel contributionsCollection { get; set; }
                    public class ContributionsCollectionModel
                    {
                        public int restrictedContributionsCount { get; set; }
                        public ContributionCalendarModel contributionCalendar { get; set; }
                        public class ContributionCalendarModel
                        {
                            public int totalContributions { get; set; }
                        }
                    }
                }
            }
            public class PageInfoModel
            {
                public string endCursor { get; set; }
                public bool hasNextPage { get; set; }
            }
        }
    }
}