using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ReservationGuid { get; set; }
        public bool IsActive { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}
