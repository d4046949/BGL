using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubApi;
using GitHubMvc.Areas.Search.Models;
using GitHubMvc.CustomAttributes;
using Newtonsoft.Json;

namespace GitHubMvc.Areas.Search.Controllers
{
    public class SearchGitHubController : Controller
    {
        private readonly IGitHubApi _gitHubApi;

        public SearchGitHubController(IGitHubApi gitHubApi)
        {
            _gitHubApi = gitHubApi;
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
                var userProfile = await _gitHubApi.GetUserProfileByName(model.Name);
                var projectListing = await _gitHubApi.GetTop5ProjectsByUrl(userProfile.RepositoryUrl);

                return View("ProjectDetails", new UserProfileViewModel
                {
                    FullName = userProfile.FullName,
                    Location = userProfile.Location,
                    AvatarPath = userProfile.AvatarPath,
                    Projects = projectListing.Select(x => new ProjectViewModel
                    {
                        Name = x.Name,
                        Rating = x.Rating
                    }).ToList()
                });
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

   

