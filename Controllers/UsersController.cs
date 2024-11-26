using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Models;
using TchauDengue.Providers;
using TchauDengue.Services;

namespace TchauDengue.Controllers
{
    [ApiController]
    [Route("/users")]
    [Authorize(Roles ="ADMIN")]
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
        [Route("get-users")]
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
                return Ok("Usu�rio Criado com Sucesso!!");
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

            if (user == null) return Unauthorized("Usu�rio incorreto!");

            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            byte[] computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("Senha incorreta!");
            }

            return new LoginDTO(userName, this.tokenService.CreateToken(user));
        }

        [HttpGet]
        [Route("get-user")]
        public async Task<ActionResult<User>> GetUser(string userName)
        {
            User user = await this.usersService.FindByUserName(userName);

            if (user == null)
            {
                return Ok("Usu�rio n�o encontrado!");
            }
            else
            {
                return Ok(new FullUserDTO(user));
            }
        }

        [HttpPut]
        [Route("update-user")]
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

        [HttpPut]
        [Route("aprove-user")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> AproveUser(int userId)
        {
            if (await this.usersService.AproveUser(userId)) return Ok("User Aproved!");

            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("add-photo")]
        public async Task<ActionResult<Picture>> AddPhoto(IFormFile photo)
        {
            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (username == null) return BadRequest("No username found in token!");

            User user = await this.usersService.FindByUserName(username);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            Picture addedPicture = await this.usersService.AddPhoto(user, photo);
            
            return Ok(addedPicture);

        }
    }
}
