using HotelBooking.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Services.Interfaces
{
    public interface IRoomService
    {
        Task<Room> GetByIdAsync(int roomId);
        Task<List<Room>> GetAllRooms();
    }
}
