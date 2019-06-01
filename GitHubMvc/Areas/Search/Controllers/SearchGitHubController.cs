using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubMvc.Areas.Search.Models;
using GitHubMvc.CustomAttributes;
using GitHubMvc.Utilities;
using Newtonsoft.Json;

namespace GitHubMvc.Areas.Search.Controllers
{

    public class UserProfile
    {
        [JsonProperty("name")]
        public string FullName { get; set; }
        public string Location { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarPath { get; set; }
    }

    public class SearchGitHubController : Controller
    {
        private readonly IConfig _configuration;

        public SearchGitHubController(IConfig configuration)
        {
            _configuration = configuration;
        }

        [Page("Search GitHub")]
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

                    var userProfileModel = JsonConvert.DeserializeObject<UserProfile>(result);
                       
                    return View("ProjectDetails", userProfileModel);

                }
            }

            return View("Index");
        }

        [Page("Project Details")]
        public ViewResult ProjectDetails()
        {
            return View();
        }
    }
}