using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationDate = new HashSet<ReservationDate>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid ReservationGuid { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ReservationDate> ReservationDate { get; set; }
    }
}
