using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
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
        public static PageUrl GetUrls(
            this IQueryCollection queryCollection,
            string hostPath,
            int totalPageCount,
            int currentPage,
            string pageQueryKey = "page")
        {
            string next = null, prev = null;
            return new PageUrl(next, prev);
        }
    }
}