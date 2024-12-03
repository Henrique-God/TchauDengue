using TchauDengue.Entities;

public class WikiPageResponseDTO
{
    public int Id { get; set; }
    public string PageText { get; set; } = string.Empty; // Default to empty string if null
    public string PageTitle { get; set; } = string.Empty; // Default to empty string if null
    public string OwnerName { get; set; } = string.Empty; // Default to empty string if null
    public DateTime CreatedAt { get; set; }
    public bool Validated { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PageHistoryResponseDTO> History { get; set; } = new List<PageHistoryResponseDTO>(); // Default to empty list if null

    // Constructor that requires both WikiPage and userName
    public WikiPageResponseDTO(WikiPage wikiPage, string userName)
    {
        if (wikiPage == null) throw new ArgumentNullException(nameof(wikiPage));

        this.Id = wikiPage.Id;
        this.PageText = wikiPage.PageText ?? string.Empty;  // Default to empty string if null
        this.PageTitle = wikiPage.PageTitle ?? string.Empty; // Default to empty string if null
        this.OwnerName = userName ?? string.Empty;  // Default to empty string if null
        this.UpdatedAt = wikiPage.UpdatedAt;
        this.Validated = wikiPage.Validated;
        
        // Ensure History is not null before proceeding with the Select operation
        this.History = wikiPage.History?.Select(ph => new PageHistoryResponseDTO(ph)).ToList() ?? new List<PageHistoryResponseDTO>(); // Default to empty list if History is null
    }

    // Constructor that only requires WikiPage
    public WikiPageResponseDTO(WikiPage wikiPage)
    {
        if (wikiPage == null) throw new ArgumentNullException(nameof(wikiPage));

        this.Id = wikiPage.Id;
        this.PageText = wikiPage.PageText ?? string.Empty; // Default to empty string if null
        this.PageTitle = wikiPage.PageTitle ?? string.Empty; // Default to empty string if null
        this.OwnerName = string.Empty; // Default to empty string if OwnerName is not set
        this.UpdatedAt = wikiPage.UpdatedAt;
        this.Validated = wikiPage.Validated;

        // Ensure History is not null before calling Select
        this.History = wikiPage.History != null
            ? wikiPage.History.Select(ph => new PageHistoryResponseDTO(ph)).ToList()
            : new List<PageHistoryResponseDTO>(); // Default to empty list if History is null
    }
}

// DTO necessary because returning WikiPage directly causes a circular reference
// WikiPage has a User, and User has a WikiPage, resulting in a deadlock
