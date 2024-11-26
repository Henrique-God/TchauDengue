using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TchauDengue.Providers;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/wikipages")]
    public class WikiPagesController: ControllerBase
    {
        private readonly DataContext DataContext;

        public WikiPagesController(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public async Task<ActionResult> CreatePage()
        {
            return Ok();
        }
    }
}
