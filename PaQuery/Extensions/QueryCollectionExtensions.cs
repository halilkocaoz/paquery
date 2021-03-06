using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using PaQuery.Enums;
using PaQuery.Models;

namespace PaQuery//.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static string ToStringQueriesWithoutPageQueryKey(this IQueryCollection queryCollection, string pageQueryKey)
        {
            var queryArray = queryCollection.ToArray();
            var areThereAnyQueryOtherThanPageQuery = queryArray.Length > 1 || queryArray.Length == 1 && pageQueryKey != queryArray[0].Key;
            if (areThereAnyQueryOtherThanPageQuery is false) return null;
            StringBuilder queries = new StringBuilder();
            foreach (var query in queryArray)
            {
                if (pageQueryKey == query.Key)
                    continue;
                queries.Append(query.Key + "=" + query.Value[0].Replace(" ", "%20") + "&");
            }
            /* 
                queries = queryOne=value&
                after the remove : queryOne=value
            */
            return $"?{queries.Remove(queries.Length - 1, 1)}";
        }
        private static int? previousPageNumber(int totalPageCount, int currentPage)
        {
            if (currentPage <= 1) return null;
            else if (currentPage > totalPageCount) return totalPageCount;

            return currentPage - 1;
        }

        private static int? nextPageNumber(int totalPageCount, int currentPage)
        {
            if (currentPage >= totalPageCount) return null;
            else if (currentPage < 1) return 1;

            return currentPage + 1;
        }
        //todo: change method name
        public static PaginationUrl GetUrls(
            this IQueryCollection queryCollection,
            string hostPath,
            int totalPageCount,
            int currentPage,
            string pageQueryKey = "page")
        {
            string nextUrl = null, prevUrl = null, queries = queryCollection.ToStringQueriesWithoutPageQueryKey(pageQueryKey);
            for (int i = -1; i <= 1; i += 2)
            {
                int? pageNumber = i == (int)PageType.Previous ? previousPageNumber(totalPageCount, currentPage) : nextPageNumber(totalPageCount, currentPage);
                if (pageNumber == null)
                    continue;

                var url = string.IsNullOrEmpty(queries) is false
                ? $"{hostPath}{queries}&{pageQueryKey}={pageNumber}"
                : $"{hostPath}?{pageQueryKey}={pageNumber}";

                if (i == (int)PageType.Previous)
                    prevUrl = url;
                else
                    nextUrl = url;
            }
            return new PaginationUrl(nextUrl, prevUrl);
        }
    }
}