using PaQuery.Enums;

namespace PaQuery.Models
{
    public class PaginationUrl
    {
        public string Next { get; private set; }
        public string Previous { get; private set; }

        public void SetUrls(PageType pageType, string queryString, string pageQueryKey, string hostPath, int selectedPageNumber)
        {
            var url = string.IsNullOrEmpty(queryString) is false
                ? $"{hostPath}{queryString}&{pageQueryKey}={selectedPageNumber}"
                : $"{hostPath}?{pageQueryKey}={selectedPageNumber}";

            if (pageType == PageType.Previous)
            {
                this.Previous = url;
            }
            else
            {
                this.Next = url;
            }
        }

        public static int? PreviousPageNumber(int totalPageCount, int currentPage)
        {
            if (totalPageCount < 1 || currentPage <= 1)
            {
                return null;
            }
            else if (currentPage > totalPageCount)
            {
                return totalPageCount;
            }

            return currentPage - 1;
        }

        public static int? NextPageNumber(int totalPageCount, int currentPage)
        {
            if (totalPageCount <= 1 || currentPage >= totalPageCount)
            {
                return null;
            }
            else if (currentPage < 1 && totalPageCount > 1)
            {
                return 2;
            }

            return currentPage + 1;
        }

        public static int? SelectPageNumber(PageType pageType, int totalPageCount, int currentPage) =>
             PageType.Previous == pageType
             ? PreviousPageNumber(totalPageCount, currentPage)
             : NextPageNumber(totalPageCount, currentPage);
    }
}