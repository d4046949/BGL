using Newtonsoft.Json;

namespace GitHubApi.Models
{
    public class Project
    {
        public string Name { get; set; }

        [JsonProperty("stargazers_count")]
        public int Rating { get; set; }
    }
}