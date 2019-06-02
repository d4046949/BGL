using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GitHubApi.Configuration;
using GitHubApi.CustomExceptions;
using GitHubApi.Models;
using Newtonsoft.Json;

namespace GitHubApi.Api
{
    public class GitHubApi : IGitHubApi, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IConfig _configuration;

        public GitHubApi(IConfig configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        public async Task<UserProfile> GetUserProfileByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();
            var url = new UriBuilder($"{_configuration.GetUrl}/users/{name}");
            return await ExecuteRequest<UserProfile>(url.ToString(),
                o => throw new ResourceNotFoundOnGitHubException("Failed to find a user profile containing " + name));
        }

        public async Task<List<Project>> GetTop5ProjectsByUrl(string userProfileRepositoryUrl)
        {
            var url = new UriBuilder(userProfileRepositoryUrl);
            return await ExecuteRequest<List<Project>>(url.ToString(),
                o => throw new ResourceNotFoundOnGitHubException(
                    "Failed to find a user profile containing " + userProfileRepositoryUrl)).ContinueWith(t =>
            {
                return t.Result.OrderByDescending(x => x.Rating).Take(5).ToList();
            });
        }


        private async Task<T> ExecuteRequest<T>(string url, Action<object> errorFunc) where T:class, new()
        {
            _client.DefaultRequestHeaders.Add("User-Agent", _configuration.GetUserAgent);

            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<T>(result);

                return obj;
            }

            errorFunc(response.StatusCode);
            return await Task.FromResult<T>(null);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}