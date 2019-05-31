using System.Web.Mvc;
using GitHubMvc;
using GitHubMvc.DependencyResolution;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StructureMap;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(StructureMapStartUp), "Start")]
[assembly: ApplicationShutdownMethod(typeof(StructureMapStartUp), "End")]

namespace GitHubMvc
{
    public static class StructureMapStartUp
    {
        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        public static void End()
        {
            StructureMapDependencyScope.Dispose();
        }

        public static void Start()
        {
            IContainer container = IoC.Initialize();
            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            DependencyResolver.SetResolver(StructureMapDependencyScope);
            DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));
        }
    }
}