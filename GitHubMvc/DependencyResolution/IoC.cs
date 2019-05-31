using StructureMap;

namespace GitHubMvc.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c => c.AddRegistry<GitHuhMvcIoCRegistry>());
        }
    }
}