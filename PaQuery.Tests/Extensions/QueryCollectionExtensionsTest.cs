using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using PaQuery.Extensions;

namespace PaQuery.Tests
{
    public class QueryCollectionExtensionsTest
    {
        private readonly DefaultHttpContext httpContext = new DefaultHttpContext();
        private const string startingOfQueryString = "?director=directorName&year=2021", pageQueryKey = "page";

        public QueryCollectionExtensionsTest()
        {
            httpContext.Request.Host = new HostString("api.testdomain.com");
            httpContext.Request.Path = new PathString("/movies");
            httpContext.Request.IsHttps = true;
        }

        [Test]
        [TestCase(startingOfQueryString + "&" + pageQueryKey)]
        public void ToStringQueriesWithoutPageQueryKey_Must_Equal_startingOfQueryString(string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={10}");
            string query = httpContext.Request.Query.ToStringQueriesWithoutPageQueryKey(pageQueryKey);

            Assert.AreEqual(query, startingOfQueryString);
        }
    }
}