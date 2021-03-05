namespace PaQuery.Models
{
    public class PaginationUrl
    {
        public string Next { get; set; }
        public string Previous { get; set; }

        public PaginationUrl(string next, string previous)
        {
            this.Next = next;
            this.Previous = previous;
        }
    }
}