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
            StringBuilder queryString = new StringBuilder();

            foreach (var query in queryArray)
            {
                if (pageQueryKey == query.Key) continue;
                queryString.Append(query.Key + "=" + query.Value[0] + "&");
            }

            return $"?{queryString.Remove(queryString.Length - 1, 1)}".Replace(" ", "%20");
        }

        public static PageInfo GetPageInfo(
            this IQueryCollection queryCollection,
            string hostPath,
            int totalPageCount,
            int currentPage,
            string pageQueryKey = "page")
        {
            var pagination = new PageInfo(); 
            string queryString = queryCollection.ToStringQueriesWithoutPageQueryKey(pageQueryKey);

            for (sbyte i = (sbyte)PageType.Previous; i <= (sbyte)PageType.Next; i += 2)
            {
                int? pageNumber = PageInfo.SelectPageNumber((PageType)i, totalPageCount, currentPage);
                if (pageNumber == null)
                    continue;

                var url = string.IsNullOrEmpty(queryString) is false
                ? $"{hostPath}{queryString}&{pageQueryKey}={pageNumber}"
                : $"{hostPath}?{pageQueryKey}={pageNumber}";

                pagination.SetUrlByPageType((PageType)i, url);
            }
            return pagination;
        }
    }
}