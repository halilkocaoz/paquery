using NUnit.Framework;
using PaQuery.Enums;
using PaQuery.Models;
using System.Collections.Generic;

namespace PaQuery.Tests
{
    public class PaginationUrlTests
    {
        [Test]
        public void SetUrlsTests()
        {
            var pageInfo = new PaginationUrl();
            const string hostPath = "api.testdomain.com", pageQueryKey = "page";
            string queryString;
            const int selectedPageNumber = 5;

            #region type1
            queryString = "?parameter=Value";
            pageInfo.SetUrls(PageType.Next, queryString, pageQueryKey, hostPath, selectedPageNumber);
            Assert.AreEqual($"{hostPath}{queryString}&{pageQueryKey}={selectedPageNumber}", pageInfo.Next);
            #endregion

            #region type2
            queryString = null;
            pageInfo.SetUrls(PageType.Next, queryString, pageQueryKey, hostPath, selectedPageNumber);
            Assert.AreEqual($"{hostPath}?{pageQueryKey}={selectedPageNumber}", pageInfo.Next);
            #endregion

            #region type3
            queryString = string.Empty;
            pageInfo.SetUrls(PageType.Next, queryString, pageQueryKey, hostPath, selectedPageNumber);
            Assert.AreEqual($"{hostPath}?{pageQueryKey}={selectedPageNumber}", pageInfo.Next);
            #endregion
        }

        private struct SubTest
        {
            public string Name;
            public int[] Values;
            public int? Expected;
        };

        private readonly List<SubTest> previousPageNumberSubTests = new List<SubTest>
        {
            new SubTest { Name = "A. RETURN NULL: totalPage > 1, currentPage = 1", Expected = null, Values = new int[] {100, 1} },
            new SubTest { Name = "B. RETURN NULL: totalPage > 1, currentPage = 0", Expected = null, Values = new int[] {100, 0} },
            new SubTest { Name = "C. RETURN NULL: totalPage > 1, currentPage = -1", Expected = null, Values = new int[] {100, -1} },
            new SubTest { Name = "D. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[] {0, 2} },

            new SubTest { Name = "E. RETURN DEFAULT VALUE (LAST PAGE NUMBER): currentPage > totalPage", Expected = 100, Values = new int[]{100, 101} },

            new SubTest { Name = "F. currentPage - 1", Expected = 99, Values = new int[]{100, 100} },
            new SubTest { Name = "G. currentPage - 1", Expected = 49, Values = new int[]{100, 50} },
            new SubTest { Name = "H. currentPage - 1", Expected = 1, Values = new int[]{100, 2} },
        };
        [Test]
        public void PreviousPageNumberTests()
        {
            foreach (var test in previousPageNumberSubTests)
            {
                var got = PaginationUrl.PreviousPageNumber(totalPageCount: test.Values[0], currentPage: test.Values[1]);
                Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
            }
        }

        private readonly List<SubTest> nextPageNumberSubTests = new List<SubTest>
        {
            new SubTest { Name = "A. RETURN NULL: totalPage > 1, currentPage >= totalPage", Expected = null, Values = new int[] {100, 100} },
            new SubTest { Name = "B. RETURN NULL: totalPage > 1, currentPage >= totalPage", Expected = null, Values = new int[] {100, 101} },
            new SubTest { Name = "C. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[] {1, -1} },
            new SubTest { Name = "D. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[] {0, -1} },
            new SubTest { Name = "E. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[] {-1, -1} },

            new SubTest { Name = "F. RETURN DEFAULT VALUE (2): totalPage > 1, currentPage < 1", Expected = 2, Values = new int[]{2, 0} },

            new SubTest { Name = "G. currentPage + 1", Expected = 2, Values = new int[]{100, 1} },
            new SubTest { Name = "H. currentPage + 1", Expected = 16, Values = new int[]{100, 15} },
            new SubTest { Name = "I. currentPage + 1", Expected = 100, Values = new int[]{100, 99} },
        };
        [Test]
        public void NextPageNumberTests()
        {
            foreach (var test in nextPageNumberSubTests)
            {
                var got = PaginationUrl.NextPageNumber(totalPageCount: test.Values[0], currentPage: test.Values[1]);
                Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
            }
        }

        private readonly List<SubTest> selectPageNumberSubTests = new List<SubTest>
        {
            new SubTest { Name = "A. Previous", Expected = 99, Values = new int[]{(int)PageType.Previous, 100, 100} },
            new SubTest { Name = "B. Next", Expected = 100, Values = new int[]{(int)PageType.Next, 100, 99} },
        };
        [Test]
        public void SelectPageNumberTests()
        {
            foreach (var test in selectPageNumberSubTests)
            {
                var got = PaginationUrl.SelectPageNumber(pageType: (PageType)test.Values[0], totalPageCount: test.Values[1], currentPage: test.Values[2]);
                Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
            }
        }
    }
}