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
            string hostPath = request.IsHttps ? $"https://{request.Host}{request.Path}" : $"http://{request.Host}{request.Path}";

            for (PageType pageType = PageType.Previous; pageType <= PageType.Next; pageType += 2)
            {
                int? pageNumber = PaginationUrl.SelectPageNumber(pageType, totalPageCount, currentPage);
                if (pageNumber == null)
                {
                    continue;
                }

                pagination.SetUrls(hostPath, queryString, pageQueryKey, pageType, (int)pageNumber);
            }
            return pagination;
        }
    }
}