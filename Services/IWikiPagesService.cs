using TchauDengue.DTOs;
using TchauDengue.Entities;

namespace TchauDengue.Services
{
    public interface IWikiPagesService
    {
        public Task<WikiPage> CreatePage(string userName, WikiPageDTO pageDto);
        public Task UpdatePage(User user, WikiPageDTO pageDto);
        
    }
}
