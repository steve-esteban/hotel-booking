using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class Room
    {
        public Room()
        {
            ReservationDate = new HashSet<ReservationDate>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual ICollection<ReservationDate> ReservationDate { get; set; }
    }
}
