using System.Collections.Generic;

namespace HotelBooking.API.Models
{
    public class AvailabilityDto : RoomDto
    {
        public List<string> AvailableDates { get; set; }
    }
}
