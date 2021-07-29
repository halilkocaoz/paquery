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

            var areThereAnyQueryOtherThanPageQuery = queryArray.Any() && pageQueryKey != queryArray[0].Key;
            if (areThereAnyQueryOtherThanPageQuery is false)
            {
                return null;
            }

            StringBuilder queryStringWithoutPageQueryKey = new StringBuilder();
            foreach (var query in queryArray)
            {
                if (pageQueryKey == query.Key)
                {
                    continue;
                }
                int queryValueCounter = 0;

                queryStringWithoutPageQueryKey.Append($"{query.Key}=");
                foreach (var value in query.Value)
                {
                    queryValueCounter++;
                    if (queryValueCounter > 1)
                    {
                        queryStringWithoutPageQueryKey.Append($"{query.Key}=");
                    }

                    queryStringWithoutPageQueryKey.Append($"{value}&");
                }
            }
            return $"?{queryStringWithoutPageQueryKey.Remove(queryStringWithoutPageQueryKey.Length - 1, 1)}".Replace(" ", "%20");
        }
    }
}