namespace TchauDengue.Entities
{
    public class PageHistory
    {
        public int Id { get; set; }
        public DateTime AddedWhen { get; set; } = DateTime.UtcNow;
        public string PageText { get; set; }
        public string PageTitle { get; set; }
        public WikiPage WikiPage { get; set; } = null!;
        public int WikiPageId { get; set; }

    }
}
