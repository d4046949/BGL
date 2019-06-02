using System.Collections.Generic;
using System.Threading.Tasks;
using GitHubApi.Models;

namespace GitHubApi.Api
{
    public interface IGitHubApi
    {
        Task<UserProfile> GetUserProfileByName(string name);
        Task<List<Project>> GetTop5ProjectsByUrl(string userProfileRepositoryUrl);
    }
}