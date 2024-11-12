namespace TchauDengue.DTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public int SocialNumber { get; set; }
        public string Password { get; set; }
    }
}
