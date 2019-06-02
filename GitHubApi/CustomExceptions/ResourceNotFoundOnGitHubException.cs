using System;

namespace GitHubApi.CustomExceptions
{
    public class ResourceNotFoundOnGitHubException : Exception
    {
        public ResourceNotFoundOnGitHubException(string message)
            : base(message)
        {
        }
        
    }
}