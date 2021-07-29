using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using PaQuery.Extensions;

namespace PaQuery.Tests
{
    public class HttpRequestExtensionsTest
    {
        private readonly DefaultHttpContext httpContext = new DefaultHttpContext();

        private const string startingOfQueryString = "?director=directorName&year=2021", pageQueryKey = "page";

        private readonly string hostPath, startingOfExpected;
        private string nextPageExpected, previousPageExpected;
        
        public HttpRequestExtensionsTest()
        {
            httpContext.Request.Host = new HostString("api.testdomain.com");
            httpContext.Request.Path = new PathString("/movies");
            httpContext.Request.QueryString = new QueryString($"{startingOfQueryString}&{pageQueryKey}={1}");
            httpContext.Request.IsHttps = true;

            hostPath = httpContext.Request.IsHttps ? $"https://{httpContext.Request.Host}{httpContext.Request.Path}" : $"http://{httpContext.Request.Host}{httpContext.Request.Path}";
            var stringQueryWithoutPageParameter = httpContext.Request.Query.ToStringQueriesWithoutPageQueryKey(pageQueryKey);
            startingOfExpected = $"{hostPath}{stringQueryWithoutPageParameter}&{pageQueryKey}=";
        }

        [Test]
        [TestCase(100, 10, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIs11_PrevIs9(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");
            var pagination = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}{selectedPage + 1}";
            previousPageExpected = $"{startingOfExpected}{selectedPage - 1}";

            Assert.AreEqual(nextPageExpected, pagination.Next);
            Assert.AreEqual(previousPageExpected, pagination.Previous);
        }

        [Test]
        [TestCase(100, 10, "?" + pageQueryKey)]
        public void NextIs11_PrevIs9_2(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");
            var pageInfo = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = $"{hostPath}{queryString}={selectedPage + 1}";
            previousPageExpected = $"{hostPath}{queryString}={selectedPage - 1}";

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }

        [Test]
        [TestCase(100, 1, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIs2_PrevIsNull(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pagination = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}2";
            previousPageExpected = null;

            Assert.AreEqual(nextPageExpected, pagination.Next);
            Assert.AreEqual(previousPageExpected, pagination.Previous);
        }

        [Test]
        [TestCase(100, 0, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIs2_PrevIsNull_2(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pagination = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}2";
            previousPageExpected = null;

            Assert.AreEqual(nextPageExpected, pagination.Next);
            Assert.AreEqual(previousPageExpected, pagination.Previous);
        }

        [Test]
        [TestCase(100, -1, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIs2_PrevIsNull_3(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pagination = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}2";
            previousPageExpected = null;

            Assert.AreEqual(nextPageExpected, pagination.Next);
            Assert.AreEqual(previousPageExpected, pagination.Previous);
        }

        [Test]
        [TestCase(100, 100, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIsNull_PrevIs99(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pagination = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = null;
            previousPageExpected = $"{startingOfExpected}99";

            Assert.AreEqual(nextPageExpected, pagination.Next);
            Assert.AreEqual(previousPageExpected, pagination.Previous);
        }

        [Test]
        [TestCase(100, 101, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIsNull_PrevIs100(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pagination = httpContext.Request.CreatePaginationUrl(totalPageCount, selectedPage);

            nextPageExpected = null;
            previousPageExpected = $"{startingOfExpected}100";

            Assert.AreEqual(nextPageExpected, pagination.Next);
            Assert.AreEqual(previousPageExpected, pagination.Previous);
        }
    }
}