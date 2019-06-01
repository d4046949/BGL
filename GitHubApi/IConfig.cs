using System;

namespace GitHubApi
{
    public interface IConfig
    {
        String GetUrl { get; }
        String GetUserAgent { get; }
    }
}