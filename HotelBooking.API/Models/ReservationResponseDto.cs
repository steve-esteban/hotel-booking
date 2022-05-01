using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models
{
    public class ReservationResponseDto
    {
        public Guid ReservationGuid { get; set; }
        public string Hotel { get; set; }
        public string Room { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
