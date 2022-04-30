using HotelBooking.Model;
using System.Threading.Tasks;

namespace HotelBooking.Services
{
    public interface IUserService
    {
        Task<User> GetByGuidAsync(string userGuid);
    }
}
