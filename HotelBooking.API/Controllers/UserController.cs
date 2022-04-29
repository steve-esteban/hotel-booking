using HotelBooking.DataAccess.EF;
using HotelBooking.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HotelBookingContext _context;

        public UserController(ILogger<WeatherForecastController> logger, HotelBookingContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = Enumerable.Empty<User>().AsQueryable();

            try
            {
                users = _context.User.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error gettin");
            }

            return users;
        }
    }
}
