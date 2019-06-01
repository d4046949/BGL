using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubMvc.Areas.Search.Models;
using GitHubMvc.Utilities;

namespace GitHubMvc.Areas.Search.Controllers
{
    public class SearchGitHubController : Controller
    {
        private readonly IConfig _configuration;

        public SearchGitHubController(IConfig configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            return View(new SearchModel());
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SearchModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", _configuration.GetUserAgent);

                    var url = new UriBuilder(_configuration.GetUrl);

                    var response = await client.GetAsync(url.ToString());
                    var result = await response.Content.ReadAsStringAsync();
                    return Content(result, "application/json");

                }
            }

            return View("Index");
        }
    }
}