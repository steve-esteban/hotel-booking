using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class UserRequestDto
    {
        [Required]
        public string UserGuid { get; set; }
    }
}
