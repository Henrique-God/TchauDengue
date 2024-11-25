using TchauDengue.DTOs;
using TchauDengue.Entities;
using TchauDengue.Providers;

namespace TchauDengue.Services
{
    public interface IUsersService
    {
        public Task<IEnumerable<UserReturnDTO>> GetUsers();
        public Task<User> Register(RegisterDTO registerDTO);
        public Task<bool> CheckLogin(string userName, string password);
        public Task<User> FindByUserName(string userName);
        public Task<User> UpdateUser(User user, UpdateUserDTO updateDto);
    }
}
