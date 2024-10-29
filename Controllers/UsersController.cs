using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using TchauDengue.Entities;
using TchauDengue.Providers;
using TchauDengue.Services;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> logger;
        private readonly IUsersService usersService;
        private readonly DataContext dataContext;

        public UsersController(ILogger<UsersController> logger, IUsersService usersService, DataContext dataContext)
        {
            this.logger = logger;
            this.usersService = usersService;
            this.dataContext = dataContext;
        }

        [HttpGet]
        [Route("/getUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            IEnumerable<User> users = await usersService.GetUsers();

            return Ok(users);
        }

        [HttpGet]
        [Route("/{id}")]
        public async Task<ActionResult<User>> FindById(int id)
        {
            User? user = await this.dataContext.Users.FindAsync(id);

            return Ok(user);
        }

        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult<User>> Register(string name, string password)
        {
            try
            {
                User user = await this.usersService.Register(name, password);
                return Ok(user);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
