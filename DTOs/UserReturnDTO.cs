using TchauDengue.Entities;

namespace TchauDengue.DTOs
{
    public class UserReturnDTO
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string City { get; set; }
        public Photo? Photo { get; set; }

        public UserReturnDTO(User user)
        {
            this.Role = user.Role;
            this.UserName = user.UserName;
            this.City = user.City;
            this.Photo = user.Photo;
        }

        public UserReturnDTO(RegisterDTO registerDTO)
        {
            this.City = registerDTO.City;
            this.UserName = registerDTO.UserName;
            this.Role = registerDTO.Role;
        }
    }
}
