namespace TchauDengue.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? ProfilePicture { get; set; }
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required string Email { get; set; }
        public required string ZipCode { get; set; }
        public string Validated { get; set; }
        public required string Role { get; set; }
        public required int SocialNumber { get; set; }

        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<WikiPage> WikiPages { get; set; } = new List<WikiPage>();
        
    }
}
