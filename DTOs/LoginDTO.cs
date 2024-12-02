using TchauDengue.Entities;

namespace TchauDengue.DTOs

{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string ProfilePicture { get; set; }

        public string Role { get; set; }  // Add Role property

        public LoginDTO(User user, string token)
        {
            this.UserName = user.UserName;
            this.Token = token;
            this.ProfilePicture = user.ProfilePicture;
            this.Role = user.Role;  // Assuming the User class has a Role property
        }

        public LoginDTO(string username, string token, string role)
        {
            this.UserName = username;
            this.Token = token;
            this.Role = role;  // Added role to constructor
        }

        
    }
}
