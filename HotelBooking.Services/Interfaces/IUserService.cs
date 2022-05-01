using HotelBooking.Model;
using System.Threading.Tasks;

namespace HotelBooking.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByGuidAsync(string userGuid);
        Task AddUserAsync(User user);
    }
}
