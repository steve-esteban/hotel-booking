using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class Hotel
    {
        public Hotel()
        {
            Room = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Room> Room { get; set; }
    }
}
