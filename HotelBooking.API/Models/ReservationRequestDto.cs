using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class ReservationRequestDto
    {
        [Required]
        public string UserGuid { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }
    }
}
