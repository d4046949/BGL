using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace GitHubApi.Tests.TestHelpers
{
    public static class AssertHelpers
    {
        public static void AssertAreEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(JsonConvert.SerializeObject(actual), (JsonConvert.SerializeObject(expected)));
        }

    }
}
