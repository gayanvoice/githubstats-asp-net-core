﻿using System.Collections.Generic;

namespace GitHubStats.Models
{
    public class GitHubModel
    {
        public int MaxNumberOfRequests { get; set; }
        public int MaxRequestSize { get; set; }
        public int ElapsedTimeInSeconds { get; set; }
        public List<string> Country { get; set; }
    }
}