using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
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

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await this.DataContext.Users.ToListAsync();
        }

        public async Task<User> Register(string name, string password)
        {
            if(name == null || password == null)
            {
                if (name == null) throw new ArgumentNullException("O campo Nome não pode ser vazio!");
                if (password == null) throw new ArgumentNullException("O campo Senha não pode ser vazio!");
            }

            if (await this.DataContext.Users.AnyAsync(u => u.UserName == name))
            {
                throw new UserAlreadyExistsException("Nome de usuário em uso!");
            } 

            using HMACSHA512 hmac = new HMACSHA512();

            User user = new User()
            {
                UserName = name,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
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
