using TchauDengue.Entities;

namespace TchauDengue.DTOs
{
    public class UserReturnDTO
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public int Id { get; set; }

        public UserReturnDTO(User user)
        {
            this.Role = user.Role;
            this.UserName = user.UserName;
            this.ProfilePicture = user.ProfilePicture;
            this.Email = user.Email;
            this.ZipCode = user.ZipCode;
            this.Id = user.Id;
        }
    }
}
