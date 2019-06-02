using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using GitHubMvc.Areas.Search.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GitHubApi.Api;
using GitHubApi.Models;
using GitHubApi.Tests.TestHelpers;
using GitHubMvc.Areas.Search.Models;

namespace GitHubMvc.Tests.Controllers
{
    [TestClass]
    public class SearchControllerTests
    {
        private readonly Mock<IGitHubApi> _mockGitHubApi = new Mock<IGitHubApi>();

        [TestMethod]
        public void Test_IndexCalledWithNewSearchModel()
        {

            SearchGitHubController controller = new SearchGitHubController(null);

            ViewResult result = controller.Index() as ViewResult;

            var model = (SearchModel) result.ViewData.Model;

            Assert.IsNull(model.Name);
        }


        [TestMethod]
        public void Test_SearchInvokedWithInvalidModelFailsValidation()
        {
            // Arrange
            var searchModel = new SearchModel();
            var context = new ValidationContext(searchModel, null, null);
            var results = new List<ValidationResult>();


            // Act
            var isModelStateValid = Validator.TryValidateObject(searchModel, context, results, true);

            Assert.IsFalse(isModelStateValid);
        }

        [TestMethod]
        public void Test_SearchInvokedWithValidModelPassesValidation()
        {
            // Arrange
            var searchModel = new SearchModel
            {
                Name = "TestProject"
            };
            var context = new ValidationContext(searchModel, null, null);
            var results = new List<ValidationResult>();


            // Act
            var isModelStateValid = Validator.TryValidateObject(searchModel, context, results, true);

            Assert.IsTrue(isModelStateValid);
        }

        [TestMethod]
        public void Test_SearchInvokedWithValidModelReturnsCorrectUserProfile()
        {
            // Arrange
            var searchModel = new SearchModel
            {
                Name = "TestProject"
            };

            _mockGitHubApi.Setup(x => x.GetUserProfileByName(It.IsAny<string>())).Returns(() =>
                Task.FromResult(new UserProfile
                {
                    AvatarPath = "pathToAvatar",
                    Location = "Location",
                    FullName = "FullName",
                    RepositoryUrl = "RepoUrl"
                }));

            _mockGitHubApi.Setup(x => x.GetTop5ProjectsByUrl(It.IsAny<string>())).Returns(() =>
                Task.FromResult(new List<Project>
                {
                    new Project
                    {
                        Name = "Project Name",
                        Rating = 5
                    }
                }));


            SearchGitHubController controller = new SearchGitHubController(_mockGitHubApi.Object);


            ViewResult result = controller.Index(searchModel).GetAwaiter().GetResult() as ViewResult;

            var model = (UserProfileViewModel) result.ViewData.Model;

            Assert.AreEqual("pathToAvatar", model.AvatarPath);
            Assert.AreEqual("Location", model.Location);
            Assert.AreEqual("FullName", model.FullName);
        }

        [TestMethod]
        public void Test_SearchInvokedWithValidModelReturnsCorrectProjectListing()
        {
            // Arrange
            var searchModel = new SearchModel
            {
                Name = "TestProject"
            };

            _mockGitHubApi.Setup(x => x.GetUserProfileByName(It.IsAny<string>())).Returns(() =>
                Task.FromResult(new UserProfile
                {
                    AvatarPath = "pathToAvatar",
                    Location = "Location",
                    FullName = "FullName",
                    RepositoryUrl = "RepoUrl"
                }));

            _mockGitHubApi.Setup(x => x.GetTop5ProjectsByUrl(It.IsAny<string>())).Returns(() =>
                Task.FromResult(new List<Project>
                {
                    new Project
                    {
                        Name = "Project Name",
                        Rating = 5
                    }
                }));


            SearchGitHubController controller = new SearchGitHubController(_mockGitHubApi.Object);


            ViewResult result = controller.Index(searchModel).GetAwaiter().GetResult() as ViewResult;

            var model = (UserProfileViewModel) result.ViewData.Model;

            List<ProjectViewModel> expected = new List<ProjectViewModel>
            {
                new ProjectViewModel
                {
                    Name = "Project Name", Rating = 5
                }
            };

            expected.AssertAreEqual(model.Projects);

        }
    }
}
