using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubStats.Models
{
    public class EdgeModel
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
