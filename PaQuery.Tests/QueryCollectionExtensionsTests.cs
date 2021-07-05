using System;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace PaQuery.Tests
{
    public class QueryCollectionExtensionsTests
    {
        private DefaultHttpContext httpContext = new DefaultHttpContext();

        private const string startingOfQueryString = "?director=directorName&year=2020", pageQueryKey = "page";

        private string hostPath, nextPageExpected, previousPageExpected, startingOfExpected;

        public QueryCollectionExtensionsTests()
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
            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}{selectedPage + 1}";
            previousPageExpected = $"{startingOfExpected}{selectedPage - 1}";

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }

        [Test]
        [TestCase(100, 10, "?" + pageQueryKey)]
        public void NextIs11_PrevIs9_2(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");
            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

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

            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}2";
            previousPageExpected = null;

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }

        [Test]
        [TestCase(100, 0, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIs2_PrevIsNull_2(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}2";
            previousPageExpected = null;

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }

        [Test]
        [TestCase(100, -1, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIs2_PrevIsNull_3(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

            nextPageExpected = $"{startingOfExpected}2";
            previousPageExpected = null;

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }

        [Test]
        [TestCase(100, 100, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIsNull_PrevIs99(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

            nextPageExpected = null;
            previousPageExpected = $"{startingOfExpected}99";

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }

        [Test]
        [TestCase(100, 101, startingOfQueryString + "&" + pageQueryKey)]
        public void NextIsNull_PrevIs99_2(int totalPageCount, int selectedPage, string queryString)
        {
            httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

            var pageInfo = httpContext.Request.Query.GetPageInfo(hostPath, totalPageCount, selectedPage);

            nextPageExpected = null;
            previousPageExpected = $"{startingOfExpected}99";

            Assert.AreEqual(nextPageExpected, pageInfo.Next);
            Assert.AreEqual(previousPageExpected, pageInfo.Previous);
        }
    }
}