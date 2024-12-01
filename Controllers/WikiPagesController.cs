using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Models;
using TchauDengue.Providers;
using TchauDengue.Services;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/wiki-pages")]
    public class WikiPagesController: ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly IUsersService usersService;
        private readonly IWikiPagesService wikiPagesService;
        private IPhotoService PhotoService { get; }

        public WikiPagesController(DataContext dataContext, IUsersService usersService, IPhotoService photoService, IWikiPagesService wikiPagesService)
        {
            this.dataContext = dataContext;
            this.usersService = usersService;
            this.PhotoService = photoService;
            this.wikiPagesService = wikiPagesService;
        }

        [HttpPost]
        [Route("create-page")]
        public async Task<ActionResult<WikiPageResponseDTO>> CreatePage([FromBody] WikiPageDTO page)
        {
            string userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userName == null) return BadRequest("No username found in token!");

            if (await dataContext.WikiPages.AnyAsync(w => w.PageTitle == page.PageTitle))
            {
                return BadRequest("Título já utilizado!");
            }

            WikiPage wikiPage = await this.wikiPagesService.CreatePage(userName, page);

            return Ok(new WikiPageResponseDTO(wikiPage, userName));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePage(int id, [FromBody] WikiPageDTO updatedPage)
        {
            var existingPage = await this.dataContext.WikiPages
                .Include(w => w.History)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (existingPage == null)
                return NotFound("Page Not Found");

            string userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userName == null) return BadRequest("No username found in token!");

            User user = await this.usersService.FindByUserName(userName);

            if (existingPage.UserId != user.Id && user.Role != Roles.ADMIN)
                return Forbid("Você não pode editar essa página");

            WikiPage page = await dataContext.WikiPages.FirstOrDefaultAsync(w => w.PageTitle == updatedPage.PageTitle);

            if (page != null && page.Id != existingPage.Id)
            {
                return BadRequest("Título já utilizado!");
            }

            var pageHistory = new PageHistory
            {
                WikiPageId = existingPage.Id,
                PageText = existingPage.PageText,
                PageTitle = existingPage.PageTitle,
                CreatedAt = existingPage.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
            existingPage.History.Add(pageHistory);

            existingPage.PageTitle = updatedPage.PageTitle;
            existingPage.PageText = updatedPage.PageText;
            existingPage.UpdatedAt = DateTime.UtcNow;

            await this.dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WikiPageResponseDTO>> GetPage(int id)
        {
            WikiPage page = await this.dataContext.WikiPages
                .Include(w => w.History)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (page == null)
                return NotFound();

            User user = this.dataContext.Users.Find(page.UserId);

            return Ok(new WikiPageResponseDTO(page, user.UserName));
        }

        [HttpPost]
        [Route("add-photo")]
        public async Task<ActionResult<Picture>> AddWikiPhoto(IFormFile photo)
        {
            var result = await this.PhotoService.AddPhotoAsync(photo);

            if (result.Error != null) throw new Exception("Unable to upload photo");

            Picture picture = new Picture
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            return Ok(picture);
        }

        [HttpGet("search-pages/{pageTitle}")]
        public async Task<ActionResult<IEnumerable<WikiPageResponseDTO>>> SearchPages(string pageTitle)
        {
            List<WikiPageResponseDTO> pages = await this.dataContext.WikiPages.Include(w => w.History).Where(p => p.Validated ==true && pageTitle.StartsWith(pageTitle.ToLower()))
                .Select(wp => new WikiPageResponseDTO(wp))
                .ToListAsync();
            
            return Ok(pages); 
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> ApprovePage(int id)
        {
            WikiPage page = await this.dataContext.WikiPages.FirstOrDefaultAsync(p => p.Id == id);

            if (page == null)
            {
                return BadRequest("Página não encontrada!");
            }

            page.Validated = true;

            await this.dataContext.SaveChangesAsync();

            return Ok(page);
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<WikiPageResponseDTO>>> GetAllPages()
        {
            List<WikiPageResponseDTO> page = await this.dataContext.WikiPages
                .Include(w => w.History)
                .Select(w => new WikiPageResponseDTO(w))
                .ToListAsync();

            if (page == null)
            {
                return BadRequest("Páginas não encontrada!");
            }

            return Ok(page);
        }
    }
}
