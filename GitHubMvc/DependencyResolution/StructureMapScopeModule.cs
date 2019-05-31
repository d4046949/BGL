namespace GitHubMvc.DependencyResolution
{
    using System.Web;
    using StructureMap.Web.Pipeline;

    public class StructureMapScopeModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => StructureMapStartUp.StructureMapDependencyScope.CreateNestedContainer();
            context.EndRequest += (sender, e) => {
                HttpContextLifecycle.DisposeAndClearAll();
                StructureMapStartUp.StructureMapDependencyScope.DisposeNestedContainer();
            };
        }
    }
}