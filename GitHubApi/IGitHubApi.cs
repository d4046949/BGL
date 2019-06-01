using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GitHubApi
{
    public class Project
    {
        public string Name { get; set; }

        [JsonProperty("stargazers_count")]
        public int Rating { get; set; }
    }

    public interface IGitHubApi
    {
        Task<UserProfile> GetUserProfileByName(string name);
        Task<List<Project>> GetTop5ProjectsByUrl(string userProfileRepositoryUrl);
    }
}