using System;
using System.Collections.Generic;

namespace HotelBooking.Models
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
