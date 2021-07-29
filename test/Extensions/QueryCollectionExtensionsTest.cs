using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using PaQuery.Extensions;

namespace PaQuery.Tests
{
    public class QueryCollectionExtensionsTest
    {
        private readonly DefaultHttpContext httpContext = new DefaultHttpContext();
        private const string startingOfQueryString = "?director=name&director=name%20surname&year=2021&title=the%20test", pageQueryKey = "page";

        public QueryCollectionExtensionsTest()
        {
            httpContext.Request.Host = new HostString("api.testdomain.com");
            httpContext.Request.Path = new PathString("/movies");
        }

        [Test]
        [TestCase(startingOfQueryString + "&" + pageQueryKey)]
        public void ToStringQueriesWithoutPageQueryKey_Must_Equal_startingOfQueryString(string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={5}");
            string query = httpContext.Request.Query.ToStringQueriesWithoutPageQueryKey(pageQueryKey);

            Assert.AreEqual(query, startingOfQueryString);
        }
    }
}