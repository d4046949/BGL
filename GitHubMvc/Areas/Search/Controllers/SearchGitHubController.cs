using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubMvc.Areas.Search.Models;

namespace GitHubMvc.Areas.Search.Controllers
{
    public class SearchGitHubController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Index(SearchModel model)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "BGL-Technicial-Test");

                var url = new UriBuilder("https://api.github.com/users/robconery");

                var response = await client.GetAsync(url.ToString());
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");

            }
        }
    }
}