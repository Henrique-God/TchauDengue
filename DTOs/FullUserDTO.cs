using TchauDengue.Entities;

namespace TchauDengue.DTOs
{
    public class FullUserDTO
    {
        public int Id { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PdfUrl { get; set; }
        public  string UserName { get; set; }
        public string Email { get; set; }
        public  string ZipCode { get; set; }
        public string Validated { get; set; }
        public string Role { get; set; }
        public int SocialNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public List<WikiPageResponseDTO> WikiPages { get; set; }

        public FullUserDTO(User user)
        {
            Id = user.Id;
            ProfilePicture = user.ProfilePicture;
            PdfUrl = user.PdfUrl;
            UserName = user.UserName;
            Email = user.Email;
            ZipCode = user.ZipCode;
            Validated = user.Validated;
            Role = user.Role;
            SocialNumber = user.SocialNumber;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
            WikiPages = user.WikiPages.Select(wp => new WikiPageResponseDTO(wp)).ToList();
        }
    }
}
