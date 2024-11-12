using TchauDengue.Entities;

namespace TchauDengue.DTOs
{
    public class UserReturnDTO
    {
        public string UserName { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public int SocialNumber { get; set; }
        public string PhoneNumber { get; set; }

        public UserReturnDTO(User user)
        {
            this.PhoneNumber = user.PhoneNumber;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.SocialNumber = user.SocialNumber;
            this.ZipCode = user.ZipCode;
        }

        public UserReturnDTO(RegisterDTO registerDTO)
        {
            this.PhoneNumber = registerDTO.PhoneNumber;
            this.UserName = registerDTO.UserName;
            this.Email = registerDTO.Email;
            this.ZipCode = registerDTO.ZipCode;
            this.SocialNumber = registerDTO.SocialNumber;
        }
    }
}
