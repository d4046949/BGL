using System.Web.Mvc;
using System.Web.Routing;

namespace GitHubMvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}",
                new { area = "Search", controller = "SearchGitHub", action = "Index"}
            ).DataTokens.Add("area", "Search");
        }
    }
}
