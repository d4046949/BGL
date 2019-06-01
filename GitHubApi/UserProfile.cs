using System;
using Newtonsoft.Json;

namespace GitHubApi
{
    public class UserProfile
    {
        [JsonProperty("name")]
        public string FullName { get; set; }
        public string Location { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarPath { get; set; }

        [JsonProperty("repos_url")]
        public string RepositoryUrl { get; set; }
    }
}