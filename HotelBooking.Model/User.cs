using System;
using System.Collections.Generic;

namespace HotelBooking.Model
{
    public partial class User
    {
        public User()
        {
            Reservation = new HashSet<Reservation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
