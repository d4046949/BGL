using System;
using System.Collections.Specialized;
using System.Configuration;

namespace GitHubMvc.Utilities
{
    public interface IConfig
    {
        String GetUrl { get; }


        String GetUserAgent { get; }
    }

public class Config : IConfig
    {
        private readonly NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        
        public String GetUrl => _appSettings["GitHubUrl"];

        public String GetUserAgent => _appSettings["UserAgents"];
    }
}