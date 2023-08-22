using NUnit.Framework;
using PaQuery.Enums;
using PaQuery.Models;
using System.Collections.Generic;

namespace PaQuery.Tests;

public class PaginationUrlTests
{
    [Test]
    public void SetUrlsTests()
    {
        var pagination = new Pagination();
        const string hostPath = "api.testdomain.com", pageQueryKey = "page";
        const int selectedPageNumber = 5;

        #region type1

        var queryString = "?parameter=Value";
        pagination.SetUrls(hostPath, queryString, pageQueryKey, PageType.Next, selectedPageNumber);
        Assert.AreEqual($"{hostPath}{queryString}&{pageQueryKey}={selectedPageNumber}", pagination.Next);

        #endregion

        #region type2

        queryString = null;
        pagination.SetUrls(hostPath, queryString, pageQueryKey, PageType.Next, selectedPageNumber);
        Assert.AreEqual($"{hostPath}?{pageQueryKey}={selectedPageNumber}", pagination.Next);

        #endregion

        #region type3

        queryString = string.Empty;
        pagination.SetUrls(hostPath, queryString, pageQueryKey, PageType.Next, selectedPageNumber);
        Assert.AreEqual($"{hostPath}?{pageQueryKey}={selectedPageNumber}", pagination.Next);

        #endregion
    }

    private struct SubTest
    {
        public string Name;
        public int[] Values;
        public int? Expected;
    };

    private readonly List<SubTest> _previousPageNumberSubTests = new()
    {
        new SubTest
        {
            Name = "A. RETURN NULL: totalPage > 1, currentPage = 1", Expected = null, Values = new[] {100, 1}
        },
        new SubTest
        {
            Name = "B. RETURN NULL: totalPage > 1, currentPage = 0", Expected = null, Values = new[] {100, 0}
        },
        new SubTest
        {
            Name = "C. RETURN NULL: totalPage > 1, currentPage = -1", Expected = null, Values = new[] {100, -1}
        },
        new SubTest
        {
            Name = "D. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null,
            Values = new[] {0, 2}
        },

        new SubTest
        {
            Name = "E. RETURN DEFAULT VALUE (LAST PAGE NUMBER): currentPage > totalPage", Expected = 100,
            Values = new[] {100, 101}
        },

        new SubTest {Name = "F. currentPage - 1", Expected = 99, Values = new[] {100, 100}},
        new SubTest {Name = "G. currentPage - 1", Expected = 49, Values = new[] {100, 50}},
        new SubTest {Name = "H. currentPage - 1", Expected = 1, Values = new[] {100, 2}},
    };

    [Test]
    public void PreviousPageNumberTests()
    {
        foreach (var test in _previousPageNumberSubTests)
        {
            var got = Pagination.PreviousPageNumber(totalPageCount: test.Values[0], currentPage: test.Values[1]);
            Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
        }
    }

    private readonly List<SubTest> _nextPageNumberSubTests = new()
    {
        new SubTest
        {
            Name = "A. RETURN NULL: totalPage > 1, currentPage >= totalPage", Expected = null,
            Values = new[] {100, 100}
        },
        new SubTest
        {
            Name = "B. RETURN NULL: totalPage > 1, currentPage >= totalPage", Expected = null,
            Values = new[] {100, 101}
        },
        new SubTest
        {
            Name = "C. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null,
            Values = new[] {1, -1}
        },
        new SubTest
        {
            Name = "D. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null,
            Values = new[] {0, -1}
        },
        new SubTest
        {
            Name = "E. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null,
            Values = new[] {-1, -1}
        },

        new SubTest
        {
            Name = "F. RETURN DEFAULT VALUE (2): totalPage > 1, currentPage < 1", Expected = 2,
            Values = new[] {2, 0}
        },

        new SubTest {Name = "G. currentPage + 1", Expected = 2, Values = new[] {100, 1}},
        new SubTest {Name = "H. currentPage + 1", Expected = 16, Values = new[] {100, 15}},
        new SubTest {Name = "I. currentPage + 1", Expected = 100, Values = new[] {100, 99}},
    };

    [Test]
    public void NextPageNumberTests()
    {
        foreach (var test in _nextPageNumberSubTests)
        {
            var got = Pagination.NextPageNumber(totalPageCount: test.Values[0], currentPage: test.Values[1]);
            Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
        }
    }

    private readonly List<SubTest> _selectPageNumberSubTests = new()
    {
        new SubTest {Name = "A. Previous", Expected = 99, Values = new[] {(int) PageType.Previous, 100, 100}},
        new SubTest {Name = "B. Next", Expected = 100, Values = new[] {(int) PageType.Next, 100, 99}},
    };

    [Test]
    public void SelectPageNumberTests()
    {
        foreach (var test in _selectPageNumberSubTests)
        {
            var got = Pagination.SelectPageNumber(pageType: (PageType) test.Values[0],
                totalPageCount: test.Values[1], currentPage: test.Values[2]);
            Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
        }
    }
}