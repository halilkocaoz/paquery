using Microsoft.AspNetCore.Http;
using PaQuery.Enums;
using PaQuery.Models;

namespace PaQuery.Extensions
{
    public static class HttpRequestExtensions
    {
        public static PaginationUrl CreatePaginationUrl(this HttpRequest request,
                                                        int totalPageCount,
                                                        int currentPage,
                                                        string pageQueryKey = "page")
        {
            var pagination = new PaginationUrl();
            string queryString = request.Query.ToStringQueriesWithoutPageQueryKey(pageQueryKey);
            string hostPath = request.IsHttps ? $"https://{request.Host}{request.Path}" : $"https://{request.Host}{request.Path}";

            for (PageType i = PageType.Previous; i <= PageType.Next; i += 2)
            {
                int? pageNumber = Models.PaginationUrl.SelectPageNumber(i, totalPageCount, currentPage);
                if (pageNumber == null)
                {
                    continue;
                }

                pagination.SetUrls(i, queryString, pageQueryKey, hostPath, (int)pageNumber);
            }
            return pagination;
        }
    }
}