using PaQuery.Enums;

namespace PaQuery.Models
{
    public class PageInfo
    {
        public string Next { get; private set; }
        public string Previous { get; private set; }

        public void SetUrlByPageType(PageType pageType, string url)
        {
            if (pageType == PageType.Previous)
                this.Previous = url;
            else
                this.Next = url;
        }

        public static int? PreviousPageNumber(int totalPageCount, int currentPage)
        {
            if (currentPage <= 1) return null;
            else if (currentPage > totalPageCount) return totalPageCount;

            return currentPage - 1;
        }

        public static int? NextPageNumber(int totalPageCount, int currentPage)
        {
            if (currentPage >= totalPageCount) return null;
            else if (currentPage < 1) return totalPageCount > 1 ? 2 : 1;

            return currentPage + 1;
        }

        public static int? SelectPageNumber(PageType pageType, int totalPageCount, int currentPage)
        {
            return PageType.Previous == pageType ? PageInfo.PreviousPageNumber(totalPageCount, currentPage) : PageInfo.NextPageNumber(totalPageCount, currentPage);
        }
    }
}