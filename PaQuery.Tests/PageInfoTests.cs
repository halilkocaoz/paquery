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
            new SubTest { Name = "null(1): totalPage = 100, currentPage = 1", Expected = null, Values = new int[]{100,1} },
            new SubTest { Name = "null(2): totalPage = 100, currentPage = 0", Expected = null, Values = new int[]{100,0} },
            new SubTest { Name = "null(3): totalPage = 100, currentPage = -1", Expected = null, Values = new int[]{100,-1} },
            new SubTest { Name = "null(4): totalPage < 1, currentPage = unimportant", Expected = null, Values = new int[]{0, 2} },

            new SubTest { Name = "currentPage must be lower than totalPageCount", Expected = 100, Values = new int[]{100,101} },

            new SubTest { Name = "currentPage - 1 (1)", Expected = 99, Values = new int[]{100,100} },
            new SubTest { Name = "currentPage - 1 (2)", Expected = 49, Values = new int[]{100,50} },
            new SubTest { Name = "currentPage - 1 (3)", Expected = 1, Values = new int[]{100,2} },
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

        [Test]
        public void NextPageNumberTests()
        {
        }

        [Test]
        public void SelectPageNumberTests()
        {
        }
    }
}