using System;

namespace GitHubApi.Configuration
{
    public interface IConfig
    {
        String GetUrl { get; }
        String GetUserAgent { get; }
    }
}