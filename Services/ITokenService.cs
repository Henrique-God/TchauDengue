using TchauDengue.Entities;

namespace TchauDengue.Services
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
