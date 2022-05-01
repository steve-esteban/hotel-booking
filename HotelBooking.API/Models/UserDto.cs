using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class UserDto
    {

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
