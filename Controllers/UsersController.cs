using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Providers;
using TchauDengue.Services;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Route("/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly IUsersService usersService;
        private readonly DataContext dataContext;
        private readonly ITokenService tokenService;

        public UsersController(IUsersService usersService, DataContext dataContext,
            ITokenService tokenService)
        {
            this.usersService = usersService;
            this.dataContext = dataContext;
            this.tokenService = tokenService;
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<ActionResult<IEnumerable<UserReturnDTO>>> GetUsers()
        {
            IEnumerable<UserReturnDTO> users = await usersService.GetUsers();

            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserReturnDTO>> FindById(int id)
        {
            User? user = await this.dataContext.Users.FindAsync(id);

            return Ok(new UserReturnDTO(user));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                User user = await this.usersService.Register(registerDTO);
                return Ok("Usuário Criado com Sucesso!!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<ActionResult<LoginDTO>> Login(string userName, string password)
        {
            User user = await this.usersService.FindByUserName(userName);

            if (user == null) return Unauthorized("Usuário incorreto!");

            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            byte[] computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("Senha incorreta!");
            }

            return new LoginDTO(userName, this.tokenService.CreateToken(user));
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<UserReturnDTO>> GetUser(string userName)
        {
            User user = await this.usersService.FindByUserName(userName);

            if (user == null)
            {
                return Ok("Usuário não encontrado!");
            }
            else
            {
                return Ok(new UserReturnDTO(user));
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UpdateUserDTO updateDTO)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username == null) return BadRequest("No username found in token!");

            User user = await this.usersService.FindByUserName(username);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (await this.usersService.UpdateUser(user, updateDTO)) return Ok("User Updated!");

            else
            {
                return BadRequest();
            }
        }
    }
}
