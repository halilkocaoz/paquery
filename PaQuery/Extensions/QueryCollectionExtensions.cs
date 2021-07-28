using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;


namespace PaQuery.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static string ToStringQueriesWithoutPageQueryKey(this IQueryCollection queryCollection, string pageQueryKey)
        {
            var queryArray = queryCollection.ToArray();

            var areThereAnyQueryOtherThanPageQuery = queryArray.Length > 1 || queryArray.Length == 1 && pageQueryKey != queryArray[0].Key;
            if (areThereAnyQueryOtherThanPageQuery is false)
            {
                return null;
            }

            StringBuilder queryString = new StringBuilder();
            foreach (var query in queryArray)
            {
                if (pageQueryKey == query.Key)
                {
                    continue;
                }
                queryString.Append(query.Key + "=" + query.Value[0] + "&");
            }

            return $"?{queryString.Remove(queryString.Length - 1, 1)}".Replace(" ", "%20");
        }
    }
}