using Microsoft.AspNetCore.Http;
using PaQuery.Enums;
using PaQuery.Models;

namespace PaQuery.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Pagination CreatePagination(this HttpRequest request,
            int totalPageCount,
            int currentPage,
            string pageQueryKey = "page")
        {
            var pagination = new Pagination(currentPage, totalPageCount);
            var queryString = request.Query.ToStringQueriesWithoutPageQueryKey(pageQueryKey);
            var hostPath = request.IsHttps
                ? $"https://{request.Host}{request.Path}"
                : $"http://{request.Host}{request.Path}";

            for (var pageType = PageType.Previous; pageType <= PageType.Next; pageType += 2)
            {
                var pageNumber = Pagination.SelectPageNumber(pageType, totalPageCount, currentPage);
                if (pageNumber == null)
                {
                    continue;
                }

                pagination.SetUrls(hostPath, queryString, pageQueryKey, pageType, (int) pageNumber);
            }

            return pagination;
        }
    }
}