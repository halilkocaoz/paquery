using NUnit.Framework;
using PaQuery.Models;
using PaQuery.Enums;
using System.Collections.Generic;

namespace PaQuery.Tests
{
    public class PageInfoTests
    {
        struct SubTest
        {
            public string Name;
            public int[] Values;
            public int? Expected;
        };

        List<SubTest> previousPageNumberSubTests = new List<SubTest>
        {
            new SubTest { Name = "A. RETURN NULL: totalPage > 1, currentPage = 1", Expected = null, Values = new int[] {100, 1} },
            new SubTest { Name = "B. RETURN NULL: totalPage > 1, currentPage = 0", Expected = null, Values = new int[] {100, 0} },
            new SubTest { Name = "C. RETURN NULL: totalPage > 1, currentPage = -1", Expected = null, Values = new int[] {100, -1} },
            new SubTest { Name = "D. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[] {0, 2} },

            new SubTest { Name = "E. RETURN DEFAULT VALUE(LAST PAGE NUMBER): currentPage > totalPage", Expected = 100, Values = new int[]{100, 101} },

            new SubTest { Name = "F. currentPage - 1", Expected = 99, Values = new int[]{100,100} },
            new SubTest { Name = "G. currentPage - 1", Expected = 49, Values = new int[]{100,50} },
            new SubTest { Name = "H. currentPage - 1", Expected = 1, Values = new int[]{100,2} },
        };

        [Test]
        public void PreviousPageNumberTests()
        {
            foreach (var test in previousPageNumberSubTests)
            {
                var got = PageInfo.PreviousPageNumber(totalPageCount: test.Values[0], currentPage: test.Values[1]);
                Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
            }
        }
        List<SubTest> nextPageNumberSubTests = new List<SubTest>
        {
            new SubTest { Name = "A. RETURN NULL: totalPage > 1, currentPage >= totalPage", Expected = null, Values = new int[] {100, 100} },
            new SubTest { Name = "B. RETURN NULL: totalPage > 1, currentPage >= totalPage", Expected = null, Values = new int[] {100, 101} },
            new SubTest { Name = "C. RETURN NULL: totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[] {0, 1} },

            new SubTest { Name = "D. RETURN DEFAULT VALUE (2): totalPage > 2 && currentPage < 1", Expected = 2, Values = new int[]{2, 0} },
            new SubTest { Name = "E. RETURN DEFAULT VALUE (2): totalPage > 2 && currentPage < 1", Expected = 2, Values = new int[]{2, -1} },

            new SubTest { Name = "F. currentPage + 1", Expected = 2, Values = new int[]{100,1} },
            new SubTest { Name = "G. currentPage + 1", Expected = 16, Values = new int[]{100,15} },
            new SubTest { Name = "H. currentPage + 1", Expected = 100, Values = new int[]{100,99} },

        };

        [Test]
        public void NextPageNumberTests()
        {
            foreach (var test in nextPageNumberSubTests)
            {
                var got = PageInfo.NextPageNumber(totalPageCount: test.Values[0], currentPage: test.Values[1]);
                Assert.AreEqual(test.Expected, got, $"Test name: {test.Name}");
            }
        }

        [Test]
        public void SelectPageNumberTests()
        {
        }
    }
}