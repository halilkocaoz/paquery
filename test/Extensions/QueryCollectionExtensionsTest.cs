using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using PaQuery.Extensions;

namespace PaQuery.Tests.Extensions;

public class QueryCollectionExtensionsTest
{
    private readonly DefaultHttpContext _httpContext = new();
    private const string StartingOfQueryString = "?director=name&director=name%20surname&year=2021&title=the%20test", PageQueryKey = "page";

    public QueryCollectionExtensionsTest()
    {
        _httpContext.Request.Host = new HostString("api.testdomain.com");
        _httpContext.Request.Path = new PathString("/movies");
    }

    [Test]
    [TestCase(StartingOfQueryString + "&" + PageQueryKey)]
    public void ToStringQueriesWithoutPageQueryKey_Must_Equal_startingOfQueryString(string queryString)
    {
        _httpContext.Request.QueryString = new QueryString($"{queryString}={5}");
        var query = _httpContext.Request.Query.ToStringQueriesWithoutPageQueryKey(PageQueryKey);

        Assert.AreEqual(query, StartingOfQueryString);
    }
}