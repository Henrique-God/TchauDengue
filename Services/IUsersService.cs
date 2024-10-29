using TchauDengue.Entities;
using TchauDengue.Providers;

namespace TchauDengue.Services
{
    public interface IUsersService
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> Register(string name, string password);
    }
}
