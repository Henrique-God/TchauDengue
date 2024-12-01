using Microsoft.EntityFrameworkCore;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Models;
using TchauDengue.Providers;

namespace TchauDengue.Services
{
    public class WikiPagesService : IWikiPagesService
    {
        public WikiPagesService(DataContext dataContext, IUsersService usersService)
        {
            this.dataContext = dataContext;
            this.usersService = usersService;
        }

        private DataContext dataContext { get; set; }
        private IUsersService usersService { get; set; }
        
        public async Task<WikiPage> CreatePage(string userName, WikiPageDTO pageDto)
        {
            User user = await this.usersService.FindByUserName(userName);

            if (user == null)
            {
                return null;
            }

            bool validation = false;

            if(user.Role == Roles.ADMIN)
            {
                validation = true;
            }

            WikiPage wikiPage = new WikiPage
            {
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Owner = user,
                PageText = pageDto.PageText,
                PageTitle = pageDto.PageTitle,
                Validated = validation,
            };


            this.dataContext.WikiPages.Add(wikiPage);
            await this.dataContext.SaveChangesAsync();

            return wikiPage;
        }

        public Task UpdatePage(User user, WikiPageDTO pageDto)
        {
            throw new NotImplementedException();
        }
    }
}
