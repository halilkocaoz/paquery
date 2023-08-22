using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using PaQuery.Extensions;

namespace PaQuery.Tests.Extensions;

public class HttpRequestExtensionsTest
{
    private readonly DefaultHttpContext _httpContext = new();

    private const string StartingOfQueryString = "?director=directorName&year=2021", PageQueryKey = "page";

    private readonly string _hostPath, _startingOfExpected;
    private string _nextPageExpected, _previousPageExpected;
        
    public HttpRequestExtensionsTest()
    {
        _httpContext.Request.Host = new HostString("api.testdomain.com");
        _httpContext.Request.Path = new PathString("/movies");
        _httpContext.Request.QueryString = new QueryString($"{StartingOfQueryString}&{PageQueryKey}={1}");
        _httpContext.Request.IsHttps = true;

        _hostPath = _httpContext.Request.IsHttps ? $"https://{_httpContext.Request.Host}{_httpContext.Request.Path}" : $"http://{_httpContext.Request.Host}{_httpContext.Request.Path}";
        var stringQueryWithoutPageParameter = _httpContext.Request.Query.ToStringQueriesWithoutPageQueryKey(PageQueryKey);
        _startingOfExpected = $"{_hostPath}{stringQueryWithoutPageParameter}&{PageQueryKey}=";
    }

    [Test]
    [TestCase(100, 10, StartingOfQueryString + "&" + PageQueryKey)]
    public void NextIs11_PrevIs9(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");
        var pagination = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = $"{_startingOfExpected}{selectedPage + 1}";
        _previousPageExpected = $"{_startingOfExpected}{selectedPage - 1}";

        Assert.AreEqual(_nextPageExpected, pagination.Next);
        Assert.AreEqual(_previousPageExpected, pagination.Previous);
    }

    [Test]
    [TestCase(100, 10, "?" + PageQueryKey)]
    public void NextIs11_PrevIs9_2(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");
        var pageInfo = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = $"{_hostPath}{queryString}={selectedPage + 1}";
        _previousPageExpected = $"{_hostPath}{queryString}={selectedPage - 1}";

        Assert.AreEqual(_nextPageExpected, pageInfo.Next);
        Assert.AreEqual(_previousPageExpected, pageInfo.Previous);
    }

    [Test]
    [TestCase(100, 1, StartingOfQueryString + "&" + PageQueryKey)]
    public void NextIs2_PrevIsNull(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

        var pagination = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = $"{_startingOfExpected}2";
        _previousPageExpected = null;

        Assert.AreEqual(_nextPageExpected, pagination.Next);
        Assert.AreEqual(_previousPageExpected, pagination.Previous);
    }

    [Test]
    [TestCase(100, 0, StartingOfQueryString + "&" + PageQueryKey)]
    public void NextIs2_PrevIsNull_2(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

        var pagination = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = $"{_startingOfExpected}2";
        _previousPageExpected = null;

        Assert.AreEqual(_nextPageExpected, pagination.Next);
        Assert.AreEqual(_previousPageExpected, pagination.Previous);
    }

    [Test]
    [TestCase(100, -1, StartingOfQueryString + "&" + PageQueryKey)]
    public void NextIs2_PrevIsNull_3(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

        var pagination = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = $"{_startingOfExpected}2";
        _previousPageExpected = null;

        Assert.AreEqual(_nextPageExpected, pagination.Next);
        Assert.AreEqual(_previousPageExpected, pagination.Previous);
    }

    [Test]
    [TestCase(100, 100, StartingOfQueryString + "&" + PageQueryKey)]
    public void NextIsNull_PrevIs99(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

        var pagination = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = null;
        _previousPageExpected = $"{_startingOfExpected}99";

        Assert.AreEqual(_nextPageExpected, pagination.Next);
        Assert.AreEqual(_previousPageExpected, pagination.Previous);
    }

    [Test]
    [TestCase(100, 101, StartingOfQueryString + "&" + PageQueryKey)]
    public void NextIsNull_PrevIs100(int totalPageCount, int selectedPage, string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={selectedPage}");

        var pagination = _httpContext.Request.CreatePagination(totalPageCount, selectedPage, PageQueryKey);

        _nextPageExpected = null;
        _previousPageExpected = $"{_startingOfExpected}100";

        Assert.AreEqual(_nextPageExpected, pagination.Next);
        Assert.AreEqual(_previousPageExpected, pagination.Previous);
    }
}