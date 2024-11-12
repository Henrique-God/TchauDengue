using TchauDengue.Entities;

namespace TchauDengue.DTOs
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }

        public LoginDTO(User user, string token)
        {
            this.UserName = user.UserName;
            this.Token = token;
        }

        public LoginDTO(string username, string token)
        {
            this.UserName = username;
            this.Token = token;
        }
    }
}
