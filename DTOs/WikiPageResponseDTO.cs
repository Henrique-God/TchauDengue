using TchauDengue.Entities;

public class WikiPageResponseDTO
{
    public int Id { get; set; }
    public string PageText { get; set; } = string.Empty;
    public string PageTitle { get; set; } = string.Empty; 
    public string OwnerName { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; }
    public bool Validated { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PageHistoryResponseDTO> History { get; set; }

    public WikiPageResponseDTO(WikiPage wikiPage, string userName)
    {
        if (wikiPage == null) throw new ArgumentNullException(nameof(wikiPage));

        this.Id = wikiPage.Id;
        this.PageText = wikiPage.PageText ?? string.Empty; 
        this.PageTitle = wikiPage.PageTitle ?? string.Empty;
        this.OwnerName = userName ?? string.Empty;  
        this.UpdatedAt = wikiPage.UpdatedAt;
        this.Validated = wikiPage.Validated;
        this.History = wikiPage.History?.Select(ph => new PageHistoryResponseDTO(ph)).ToList() ?? new List<PageHistoryResponseDTO>();
    }

    public WikiPageResponseDTO(WikiPage wikiPage)
    {
        if (wikiPage == null) throw new ArgumentNullException(nameof(wikiPage));

        this.Id = wikiPage.Id;
        this.PageText = wikiPage.PageText ?? string.Empty; 
        this.PageTitle = wikiPage.PageTitle ?? string.Empty; 
        this.OwnerName = string.Empty; 
        this.UpdatedAt = wikiPage.UpdatedAt;
        this.Validated = wikiPage.Validated;
        this.UpdatedAt = wikiPage.UpdatedAt;
        this.History = (wikiPage.History ?? new List<PageHistory>())
        .Select(ph => new PageHistoryResponseDTO(ph))
        .ToList();
    }
}
