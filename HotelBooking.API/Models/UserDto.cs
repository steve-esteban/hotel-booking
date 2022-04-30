using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class UserDto
    {
        public UserDto() { }
        public UserDto(string userGuid, string fullName, string email, string phone)
        {
            UserGuid = userGuid;
            FullName = fullName;
            Email = email;
            Phone = phone;
        }

        [Required]
        public string UserGuid { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
    }
}
