using TchauDengue.Entities;

namespace TchauDengue.DTOs
{
    public class UpdateUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Neighbor {  get; set; }
        public string PhoneNumber { get; set; }
        public Photo Photo { get; set; }

    }
}
