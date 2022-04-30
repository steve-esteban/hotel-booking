using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class ReservationDate
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }

        public virtual Room Room { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
