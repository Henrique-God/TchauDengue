﻿using TchauDengue.Entities;

public class WikiPageResponseDTO
{
    public int Id { get; set; }
    public string PageText { get; set; }
    public string PageTitle { get; set; }
    public string OwnerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PageHistoryResponseDTO> History { get; set; }

public WikiPageResponseDTO(WikiPage wikiPage, string userName)
{
    if (wikiPage == null) throw new ArgumentNullException(nameof(wikiPage));
    
    this.Id = wikiPage.Id;
    this.PageText = wikiPage.PageText ?? "";
    this.PageTitle = wikiPage.PageTitle ?? "";
    this.OwnerName = userName ?? "";
    this.UpdatedAt = wikiPage.UpdatedAt;
    this.History = (wikiPage.History ?? new List<PageHistory>())
        .Select(ph => new PageHistoryResponseDTO(ph))
        .ToList();
}


    public WikiPageResponseDTO(WikiPage wikiPage)
    {
        this.Id = wikiPage.Id;
        this.PageText = wikiPage.PageText;
        this.PageTitle = wikiPage.PageTitle;
        this.OwnerName = "";
        this.UpdatedAt = wikiPage.UpdatedAt;
        this.History = wikiPage.History
            .Select(ph => new PageHistoryResponseDTO(ph))
            .ToList();
    }
}

//DTO necessário pq se vc retorna a WikiPage direta ela fica em um loopCiclico
//PQ na wikipage tem um User, e no User tem uma WikiPage, resultando em um deadLock
