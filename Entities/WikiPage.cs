using System.ComponentModel.DataAnnotations.Schema;

namespace TchauDengue.Entities
{
    [Table("WikiPages")]
    public class WikiPage
    {
        public int Id { get; set; }
        public string PageText { get; set; }
        public string PageTitle { get; set; }
        public User Owner { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PageHistory> History {  get; set; }
    }
}
