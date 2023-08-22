using PaQuery.Enums;

namespace PaQuery.Models
{
    public class Pagination
    {
        public Pagination()
        {
            
        }
        public Pagination(int currentPage, int totalPages)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public string Next { get; private set; }
        public string Previous { get; private set; }

        public void SetUrls(string hostPath, string queryString, string pageQueryKey, PageType pageType, int selectedPageNumber)
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

            if (currentPage > totalPageCount)
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

            if (currentPage < 1)
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