using System.Net.Http;
using StructureMap;

namespace GitHubApi
{
    public class GitHubApiRegistry : Registry
    {
        public GitHubApiRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.Assembly("GitHubApi");
            });

            For<HttpClient>().Use(x => new HttpClient());
        }
    }
}