using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GitHubApi.Api;
using GitHubApi.Configuration;
using GitHubApi.Models;
using GitHubApi.Tests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitHubApi.Tests
{
    public class MockHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException("THIS METHOD NEEDS TO BE IMPLEMENTED IN THE SETUP OF MY TESTS");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }

    [TestClass]
    public class GitHubApiTests
    {
        private IGitHubApi _api;
        private readonly Mock<IConfig> _mockConfiguration;

        private readonly Mock<MockHandler> _httpHandler;

        public GitHubApiTests()
        {
            _mockConfiguration = new Mock<IConfig>();
            _mockConfiguration.Setup(x => x.GetUrl).Returns("http://my-test-url.com");
            _mockConfiguration.Setup(x => x.GetUserAgent).Returns("my-user-agent-1");

            _httpHandler = new Mock<MockHandler>
            {
                CallBase = true
            };
        }

        [TestInitialize]
        public void Setup()
        {
            _api = new Api.GitHubApi(_mockConfiguration.Object, new HttpClient(_httpHandler.Object));
        }

        [TestMethod]

        public void Test_GetUserByNameWhereNameIsEmptyShouldThrowNullArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _api.GetUserProfileByName(string.Empty).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void Test_GetUserByNameWhereNameIsNullShouldThrowNullArgumentException()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _api.GetUserProfileByName(null).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void Test_GetTop5ProjectsByUrlReturnsEmptyListWhenNoProjectsExist()
        {
            //setup
            _httpHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[]")
            });

            //Do
            var result = _api.GetTop5ProjectsByUrl("http://my-test-project-that-does-not-exist").GetAwaiter()
                .GetResult();

            //Verify
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_GetTop5ProjectsByUrlReturnsCorrectlyOrderedResults()
        {
            //setup
            _httpHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"id\": 59138,\"name\": \"my-test-project\", \"stargazers_count\": 2}," +
                                            "{\"id\": 59138,\"name\": \"Blockchain-parser\", \"stargazers_count\": 1}, " +
                                            "{\"id\": 59138,\"name\": \"Mulesworth\", \"stargazers_count\": 5}," +
                                            "{\"id\": 59138,\"name\": \"My-Awesome-FileParser\", \"stargazers_count\": 3}, " +
                                            "{\"id\": 59138,\"name\": \"My-Awesome-FileParser-remix\", \"stargazers_count\": 3}]")
            });

            //Do
            var result = _api.GetTop5ProjectsByUrl("http://my-test-project-that-does-not-exist").GetAwaiter()
                .GetResult();

            //Verify

            var expectedResult = new List<Project>
            {
                new Project {Name = "Mulesworth", Rating = 5},
                new Project {Name = "My-Awesome-FileParser", Rating = 3},
                new Project {Name = "My-Awesome-FileParser-remix", Rating = 3},
                new Project {Name = "my-test-project", Rating = 2},
                new Project {Name = "Blockchain-parser", Rating = 1}
            };

            expectedResult.AssertAreEqual(result);
        }

        [TestMethod]
        public void Test_GetTop5ProjectsByUrlReturnsOnly5ElementsWhenResponseContainsMoreThan5()
        {
            //setup
            _httpHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"id\": 59138,\"name\": \"my-test-project\", \"stargazers_count\": 2}," +
                                            "{\"id\": 59138,\"name\": \"Blockchain-parser\", \"stargazers_count\": 1}, " +
                                            "{\"id\": 59138,\"name\": \"Mulesworth\", \"stargazers_count\": 5}," +
                                            "{\"id\": 59138,\"name\": \"My-Awesome-FileParser\", \"stargazers_count\": 3}, " +
                                            "{\"id\": 59138,\"name\": \"My-Awesome-FileParser-remix\", \"stargazers_count\": 3}," +
                                            "{\"id\": 59138,\"name\": \"My-Awesome-FileParser-remix-01\", \"stargazers_count\": 0}]")
            });

            //Do
            var result = _api.GetTop5ProjectsByUrl("http://my-test-project-that-does-not-exist").GetAwaiter()
                .GetResult();

            //Verify

            var expectedResult = new List<Project>
            {
                new Project {Name = "Mulesworth", Rating = 5},
                new Project {Name = "My-Awesome-FileParser", Rating = 3},
                new Project {Name = "My-Awesome-FileParser-remix", Rating = 3},
                new Project {Name = "my-test-project", Rating = 2},
                new Project {Name = "Blockchain-parser", Rating = 1}
            };

            expectedResult.AssertAreEqual(result);
        }

        [TestMethod]
        public void Test_GetUserProfileByNameWhereNameDoesNotExist()
        {
            //Setup
            _httpHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"message\": \"Not Found\",\"documentation_url\": \"https://developer.github.com/v3/users/#get-a-single-user\"}")
            });

            //Do
            var result = _api.GetUserProfileByName("My-awesome-project").GetAwaiter().GetResult();

            //Verify
            


        }
    }
}