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
        public Task<bool> UpdateUser(User user, UpdateUserDTO updateDto);
        public Task<Picture> AddPhoto(User user, IFormFile photo);
        public Task<Picture> AddDocument(User user, IFormFile document);
        public Task<bool> AproveUser(int userId);
        public Task<byte[]> GetDocument(User user, string pdfUrl);
    }
}
