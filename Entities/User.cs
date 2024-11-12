namespace TchauDengue.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string ZipCode { get; set; }
        public required int SocialNumber { get; set; }
        
    }
}
