using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TchauDengue.Providers;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/pages")]
    public class WikiPagesController: ControllerBase
    {
        private readonly DataContext DataContext;

        public WikiPagesController(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }
    }
}
