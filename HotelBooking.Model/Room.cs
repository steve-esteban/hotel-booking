using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class Room
    {
        public Room()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
