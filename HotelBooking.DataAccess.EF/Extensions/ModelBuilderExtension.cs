using HotelBooking.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.DataAccess.EF.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasData(new Hotel
            {
                Id = 1,
                Name = "Cancun Hotel"
            });

            modelBuilder.Entity<Room>().HasData(new Room
            {
                Id = 1,
                Name = "Room 101",
                HotelId = 1
            });
        }
    }
}
