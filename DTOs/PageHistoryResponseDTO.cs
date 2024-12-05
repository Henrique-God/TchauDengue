using TchauDengue.Entities;

public class PageHistoryResponseDTO
{
    public int Id { get; set; }
    public string PageText { get; set; }
    public string PageTitle { get; set; }
    public DateTime CreatedAt { get; set; }

    public PageHistoryResponseDTO(PageHistory pageHistory)
    {
        this.Id = pageHistory.Id;
        this.PageText = pageHistory.PageText;
        this.PageTitle = pageHistory.PageTitle;
        this.CreatedAt = pageHistory.CreatedAt;
    }
}