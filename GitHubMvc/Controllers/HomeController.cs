using System.Web.Mvc;
using GitHubMvc.Utilities;

namespace GitHubMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfig _configuration;

        public HomeController(IConfig configuration)
        {
            _configuration = configuration;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}