namespace PaQuery.Models
{
    public class PageUrl
    {
        public string Next { get; set; }
        public string Previous { get; set; }

        public PageUrl(string next, string previous)
        {
            this.Next = next;
            this.Previous = previous;
        }
    }
}