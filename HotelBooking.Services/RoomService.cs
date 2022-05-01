using HotelBooking.DataAccess.EF.Repositories;
using HotelBooking.Model;
using HotelBooking.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Services
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Room> _roomRepository;

        public RoomService(IRepositoryFactory repositoryFactory)
        {
            _roomRepository = repositoryFactory.GetRepository<Room>();
        }

        public async Task<Room> GetByIdAsync(int roomId)
        {
            return await _roomRepository.SingleOrDefaultAsync(x => x.Id == roomId, new List<string>() { "Hotel" });
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return await _roomRepository.GetAllAsync(new List<string>() { "Hotel" });
        }

    }
}
