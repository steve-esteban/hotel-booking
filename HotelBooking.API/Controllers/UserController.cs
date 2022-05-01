using AutoMapper;
using HotelBooking.API.Models;
using HotelBooking.Model;
using HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> GetUserInformationAsync([FromHeader(Name = "user-guid")] string userGuid)
        {
            try
            {
                var user = await _userService.GetByGuidAsync(userGuid);
                if (user == null)
                    return NotFound($"User '{userGuid}' not found");

                var userDto = _mapper.Map<UserDto>(user);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user '{userGuid}'");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpPost()]
        public async Task<IActionResult> CreateUserAsync([FromBody][Bind] UserDto userDto)
        {
            _logger.LogInformation($"Creating user: {Newtonsoft.Json.JsonConvert.SerializeObject(userDto)}");

            try
            {
                var existingUser = await _userService.GetByGuidAsync(userDto.UserGuid);
                if (existingUser != null)
                {
                    _logger.LogError($"Error creating user: {Newtonsoft.Json.JsonConvert.SerializeObject(userDto)}; User already exists");
                    return Conflict($"User with Guid {userDto.UserGuid} already exist");
                }

                var user = _mapper.Map<User>(userDto);
                await _userService.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating user: {Newtonsoft.Json.JsonConvert.SerializeObject(userDto)}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

            return Ok($"User created with Guid '{userDto.UserGuid}'");
        }
    }
}
