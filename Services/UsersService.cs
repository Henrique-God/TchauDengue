using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Exceptions;
using TchauDengue.Providers;

namespace TchauDengue.Services
{
    public class UsersService: IUsersService
    {
        public UsersService(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        private DataContext DataContext { get; }

        public async Task<IEnumerable<UserReturnDTO>> GetUsers()
        {
            List<User> users = await this.DataContext.Users.ToListAsync();
            List<UserReturnDTO> userReturnDTOs = new List<UserReturnDTO>();
            foreach (var user in users)
            {
                userReturnDTOs.Add(new UserReturnDTO(user));
            }
            return userReturnDTOs;
        }

        public async Task<User> Register(RegisterDTO registerDTO)
        {

            if (await this.DataContext.Users.AnyAsync(u => u.UserName == registerDTO.UserName))
            {
                throw new UserAlreadyExistsException("Nome de usuário em uso!");
            }
            if (await this.DataContext.Users.AnyAsync(u => u.Email == registerDTO.Email))
            {
                throw new UserAlreadyExistsException("Email em uso!");
            }
            if (await this.DataContext.Users.AnyAsync(u => u.SocialNumber == registerDTO.SocialNumber))
            {
                throw new UserAlreadyExistsException("CPF em uso!");
            }
            if (await this.DataContext.Users.AnyAsync(u => u.PhoneNumber == registerDTO.PhoneNumber))
            {
                throw new UserAlreadyExistsException("Telefone em uso!");
            }
            using HMACSHA512 hmac = new HMACSHA512();

            User user = new User()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                SocialNumber = registerDTO.SocialNumber,
                ZipCode = registerDTO.ZipCode,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            this.DataContext.Users.Add(user);
            await this.DataContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> CheckLogin(string userName, string password)
        {
            User user = await this.DataContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return false;

            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);

            byte[] computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return false;
            }

            return true;
        }
    }
}
