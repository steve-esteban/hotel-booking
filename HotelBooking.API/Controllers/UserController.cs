using HotelBooking.API.Models;
using HotelBooking.DataAccess.EF;
using HotelBooking.Model;
using HotelBooking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly HotelBookingContext _context;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, HotelBookingContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        [HttpPost("GetUserInformation")]
        public async Task<IActionResult> GetUserInformationAsync([FromBody][Bind] UserRequestDto userRequestDto)
        {
            UserDto userDto;
            try
            {
                var user = await _userService.GetByGuidAsync(userRequestDto.UserGuid);
                if (user == null)
                {
                    _logger.LogError($"User '{userRequestDto.UserGuid}' not found");
                    return NotFound($"User '{userRequestDto.UserGuid}' not found");
                }

                userDto = new UserDto(user.UserGuid, user.FullName, user.Email, user.Phone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user '{userRequestDto.UserGuid}'");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(userDto);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUserAsync([FromBody][Bind] UserDto userDto)
        {
            _logger.LogInformation($"Creating user: {Newtonsoft.Json.JsonConvert.SerializeObject(userDto)}");

            var user = new User()
            {
                UserGuid = userDto.UserGuid,
                FullName = userDto.FullName,
                Email = userDto.Email,
                Phone = userDto.Phone
            };
            try
            {
                var existingUser = await _context.User.FirstOrDefaultAsync(x => x.UserGuid == userDto.UserGuid);
                if (existingUser != null)
                {
                    _logger.LogError($"Error creating user: {Newtonsoft.Json.JsonConvert.SerializeObject(userDto)}; User already exists");
                    return Conflict($"User with Guid {userDto.UserGuid} already exist");
                }

                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating user: {Newtonsoft.Json.JsonConvert.SerializeObject(userDto)}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return Ok($"User created with Guid '{user.UserGuid}'");
        }
    }
}
