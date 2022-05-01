using HotelBooking.DataAccess.EF.Repositories;
using HotelBooking.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HotelBooking.Services.Interfaces;

namespace HotelBooking.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IRepositoryFactory repositoryFactory,
            ILogger<UserService> logger)
        {
            _userRepository = repositoryFactory.GetRepository<User>();
            _logger = logger;
        }

        public async Task<User> GetByGuidAsync(string userGuid)
        {
            return await _userRepository.SingleOrDefaultAsync(x => x.UserGuid == userGuid);
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Saving User");
                throw;
            }
        }

    }
}
