
using GitHubApi;
using GitHubApi.Configuration;
using StructureMap;

namespace GitHubMvc.DependencyResolution
{
    public class GitHuhMvcIoCRegistry : Registry
    {
        public GitHuhMvcIoCRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.Assembly("GitHubMvc");
                x.WithDefaultConventions();
            });

            For<IConfig>().Use(new Config()).Singleton();
        }
   }
}