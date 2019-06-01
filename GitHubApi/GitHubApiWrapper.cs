using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace GitHubApi
{
    public class GitHubApi : IGitHubApi
    {
        private HttpClient _client;
        private readonly IConfig _configuration;

        public GitHubApi(IConfig configuration)
        {
            _configuration = configuration;
        }

        public async Task<UserProfile> GetUserProfileByName(string name)
        {
            var url = new UriBuilder(_configuration.GetUrl);
            return await ExecuteRequest<UserProfile>(url.ToString());
        }

        public async Task<List<Project>> GetTop5ProjectsByUrl(string userProfileRepositoryUrl)
        {
            var url = new UriBuilder(userProfileRepositoryUrl);
            return await ExecuteRequest<List<Project>>(url.ToString()).ContinueWith(t =>
            {
                return t.Result.OrderByDescending(x => x.Rating).Take(5).ToList();
            });
        }


        private async Task<T> ExecuteRequest<T>(string url)
        {
            using (_client = new HttpClient())
            {
                _client.DefaultRequestHeaders.Add("User-Agent", _configuration.GetUserAgent);

                var response = await _client.GetAsync(url.ToString());
                var result = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<T>(result);

                return obj;

            }
        }
    }
}