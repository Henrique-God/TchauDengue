using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Providers;
using TchauDengue.Services;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Route("/api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> logger;
        private readonly IUsersService usersService;
        private readonly DataContext dataContext;
        private readonly ITokenService tokenService;

        public UsersController(ILogger<UsersController> logger, IUsersService usersService, DataContext dataContext,
            ITokenService tokenService)
        {
            this.logger = logger;
            this.usersService = usersService;
            this.dataContext = dataContext;
            this.tokenService = tokenService;
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
        [AllowAnonymous]
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

        [HttpPost]
        [AllowAnonymous]
        [Route("/login")]
        public async Task<ActionResult<UserDTO>> Login(string userName, string password)
        {
            User user = await this.dataContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            
            if (user == null) return Unauthorized("Usuário incorreto!");

            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            byte[] computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("Senha incorreta!");
            }

            return new UserDTO
            {
                UserName = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };
        }
    }
}
