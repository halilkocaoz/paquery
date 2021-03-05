using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using PaQuery.Enums;
using PaQuery.Models;

namespace PaQuery//.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static string toStringQueriesWithoutPageQueryKey(this IQueryCollection queryCollection, string pageQueryKey)
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
            return "?" + queries;
        }

        //todo: change method name
        public static PaginationUrl GetUrls(
            this IQueryCollection queryCollection,
            string hostPath,
            int totalPageCount,
            int currentPage,
            string pageQueryKey = "page")
        {
            string nextUrl = null, prevUrl = null, queries = queryCollection.toStringQueriesWithoutPageQueryKey(pageQueryKey);
            for (int i = -1; i <= 1; i += 2)
            {
                #region selectPageNumber //todo refactor 
                int? pageNumber = 0;
                if (i == (int)PageType.Previous)
                {
                    if (currentPage <= 1) pageNumber = null;
                    else if (currentPage > totalPageCount) pageNumber = totalPageCount;

                    pageNumber = currentPage + i;
                }
                else if (i == (int)PageType.Next)
                {
                    if (currentPage >= totalPageCount) pageNumber = null;
                    else if (currentPage < 1) pageNumber = 1;

                    pageNumber = currentPage + i;
                }
                #endregion
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