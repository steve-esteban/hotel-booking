using HotelBooking.API.Extensions;
using HotelBooking.API.Models;
using HotelBooking.DataAccess.EF.Constants;
using HotelBooking.Model;
using HotelBooking.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using System.Text.Json;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public BookingController(ILogger<BookingController> logger, IUserService userService,
            IRoomService roomService, IReservationService reservationService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _roomService = roomService;
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpGet("rooms")]
        public async Task<ActionResult<List<RoomDto>>> GetAllRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllRooms();
                var roomDtoList = rooms.Select(x => _mapper.Map<RoomDto>(x));
                return Ok(roomDtoList);
            }
            catch (Exception)
            {
                _logger.LogError("Error getting available rooms");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting available rooms");
            }
        }

        [HttpGet("room/{roomId:int}/availability")]
        public async Task<ActionResult<AvailabilityDto>> GetAvailabilityAsync([FromRoute] int roomId)
        {
            try
            {
                var room = await GetRoom(roomId);
                var availableDates = await _reservationService.GetAvailableDatesAsync(room.Id);

                var availability = _mapper.Map<AvailabilityDto>(room);
                availability.AvailableDates = availableDates.Select(x => x.ToStringDefault()).ToList();

                return Ok(availability);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting availability for room {roomId}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("room/{roomId:int}/reservation")]
        public async Task<IActionResult> MakeReservation([FromRoute] int roomId, [FromHeader(Name = "user-guid")] string userGuid, [FromBody][Bind] ReservationRequestDto reservationDto)
        {
            _logger.LogInformation($"Starting making reservation: {JsonSerializer.Serialize(reservationDto)}");

            var validation = ValidateReservationDates(reservationDto);
            if (validation.StatusCode != StatusCodes.Status200OK)
            {
                _logger.LogError("Error making reservation");
                return validation;
            }

            try
            {
                var room = await GetRoom(roomId);
                var user = await _userService.GetByGuidAsync(userGuid);
                if (user == null)
                {
                    _logger.LogError($"Error making reservation. User {userGuid} not found");
                    return NotFound($"User {userGuid} not found. Please register the user with the user/CreateUser endpoint.");
                }

                var bookedDates = await _reservationService.GetListOfBookedDatesAsync(room.Id);

                var reservationDates = new List<ReservationDate>();

                for (var date = reservationDto.StartDate; date <= reservationDto.EndDate; date = date.AddDays(1))
                {
                    if (bookedDates.Any(x => x.Date == date.Date))
                    {
                        _logger.LogError($"Error making reservation. Date {date.Date} is already booked");
                        return Conflict($"Date {date.Date.ToStringDefault()} is already booked");
                    }

                    reservationDates.Add(new ReservationDate()
                    {
                        RoomId = room.Id,
                        Date = date.Date
                    });
                }

                var reservation = new Reservation()
                {
                    ReservationGuid = Guid.NewGuid(),
                    UserId = user.Id,
                    ReservationDate = reservationDates
                };

                await _reservationService.SaveReservationAsync(reservation);

                _logger.LogInformation($"Finish making reservation: {JsonSerializer.Serialize(reservation.ReservationGuid)}");
                return Ok($"Booked reservation for user {user.UserGuid}. Reservation Id: {reservation.ReservationGuid}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error making reservation", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut("reservation/{reservationGuid:Guid}")]
        public async Task<IActionResult> UpdateReservation([FromHeader(Name = "user-guid")] string userGuid, [FromBody][Bind] ReservationRequestDto reservationDto, Guid reservationGuid)
        {
            _logger.LogInformation($"Starting making reservation: {JsonSerializer.Serialize(reservationDto)}");

            var validation = ValidateReservationDates(reservationDto);
            if (validation.StatusCode != StatusCodes.Status200OK)
            {
                _logger.LogError("Error making reservation");
                return validation;
            }

            try
            {
                var user = await _userService.GetByGuidAsync(userGuid);
                if (user == null)
                    return NotFound($"User '{userGuid}' not found");

                var reservation = await _reservationService.GetReservationAsync(user.Id, reservationGuid);
                if (reservation == null)
                    return NotFound($"Reservation {reservationGuid} for user '{userGuid}' not found");

                if (reservation.ReservationDate?.Min(x => x.Date) <= _reservationService.GetRoomDateTimeNow().Date)
                    return Conflict("Can't modify an old reservation");

                var roomId = _reservationService.GetReservationRoomId(reservation);
                var bookedDates = await _reservationService.GetListOfBookedDatesAsync(roomId);

                var existingReservationDates = reservation.ReservationDate?.ToList();
                var reservationDatesToIgnore = new List<ReservationDate>();
                var reservationDatesToAdd = new List<ReservationDate>();

                for (var dateToUpdate = reservationDto.StartDate; dateToUpdate <= reservationDto.EndDate; dateToUpdate = dateToUpdate.AddDays(1))
                {

                    if (existingReservationDates != null && existingReservationDates.Any(x => x.Date.Date == dateToUpdate.Date))
                    {
                        // Ignore ReservationDate that has already been added
                        reservationDatesToIgnore.Add(existingReservationDates.FirstOrDefault(x => (x.RoomId == roomId && x.Date.Date == dateToUpdate.Date)));
                        continue;
                    }

                    if (bookedDates.Any(x => x.Date == dateToUpdate.Date) && !existingReservationDates.Any(x => x.Date.Date == dateToUpdate.Date))
                    {
                        _logger.LogError($"Error updating reservation. Date {dateToUpdate.Date} is already booked");
                        return Conflict($"Date {dateToUpdate.Date.ToStringDefault()} is already booked");
                    }

                    reservationDatesToAdd.Add(new ReservationDate()
                    {
                        ReservationId = reservation.Id,
                        RoomId = roomId,
                        Date = dateToUpdate.Date
                    });
                }

                var reservationDatesToRemove = existingReservationDates.Except(reservationDatesToIgnore);
                await _reservationService.UpdateReservationDateAsync(reservationDatesToRemove, reservationDatesToAdd);

                _logger.LogInformation($"Finish updating reservation: {JsonSerializer.Serialize(reservation.ReservationGuid)}");
                return Ok($"Updated reservation for user {user.UserGuid}. Reservation Id: {reservation.ReservationGuid}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error making reservation", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("reservation")]
        public async Task<ActionResult<List<ReservationResponseDto>>> GetUserReservations([FromHeader(Name = "user-guid")] string userGuid)
        {
            try
            {
                var reservationDtoList = new List<ReservationResponseDto>();
                var user = await _userService.GetByGuidAsync(userGuid);
                if (user == null)
                    return NotFound($"User '{userGuid}' not found");

                var rooms = await _roomService.GetAllRooms();
                var reservationList = await _reservationService.GetReservationByUserIdAsync(user.Id);
                if (reservationList == null)
                    return NotFound($"Reservations for user '{userGuid}' not found");

                foreach (var reservation in reservationList)
                {
                    var reservationDto = reservation.ReservationDate.GroupBy(x => new { x.ReservationId, x.RoomId },
                         (key, group) =>
                             new ReservationResponseDto()
                             {
                                 ReservationGuid = reservation.ReservationGuid,
                                 StartDate = group.Min(x => x.Date).Date.ToStringDefault(),
                                 EndDate = group.Max(x => x.Date).Date.ToStringDefault(),
                                 Room = rooms.FirstOrDefault(x => x.Id == key.RoomId)?.Name,
                                 Hotel = rooms.FirstOrDefault(x => x.Id == key.RoomId)?.Hotel.Name
                             });

                    reservationDtoList.AddRange(reservationDto);

                }
                return Ok(reservationDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reservations for user '{userGuid}'");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpGet("reservation/{reservationGuid:Guid}")]
        public async Task<ActionResult<ReservationResponseDto>> GetReservations([FromHeader(Name = "user-guid")] string userGuid, [FromRoute] Guid reservationGuid)
        {
            try
            {
                if (userGuid == string.Empty)
                    return BadRequest($"user-guid is required");

                var reservationDtoList = new List<ReservationResponseDto>();
                var user = await _userService.GetByGuidAsync(userGuid);
                if (user == null)
                    return NotFound($"User '{userGuid}' not found");

                var rooms = await _roomService.GetAllRooms();
                var reservation = await _reservationService.GetReservationAsync(user.Id, reservationGuid);
                if (reservation == null)
                    return NotFound($"Reservation {reservationGuid} for user '{userGuid}' not found");

                var reservationDto = reservation.ReservationDate.GroupBy(x => new { x.ReservationId, x.RoomId },
                    (key, group) =>
                        new ReservationResponseDto()
                        {
                            ReservationGuid = reservation.ReservationGuid,
                            StartDate = group.Min(x => x.Date).Date.ToStringDefault(),
                            EndDate = group.Max(x => x.Date).Date.ToStringDefault(),
                            Room = rooms.FirstOrDefault(x => x.Id == key.RoomId)?.Name,
                            Hotel = rooms.FirstOrDefault(x => x.Id == key.RoomId)?.Hotel.Name
                        });

                reservationDtoList.AddRange(reservationDto);

                return Ok(reservationDtoList.FirstOrDefault());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reservations for user '{userGuid}'");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

        [HttpDelete("reservation/{reservationGuid:Guid}")]
        public async Task<IActionResult> RemoveReservations([FromHeader(Name = "user-guid")] string userGuid, [FromRoute] Guid reservationGuid)
        {
            _logger.LogInformation($"Starting removing reservation {reservationGuid} for user {userGuid}");
            try
            {
                if (userGuid == string.Empty)
                    return BadRequest($"user-guid is required");
                var user = await _userService.GetByGuidAsync(userGuid);
                if (user == null)
                    return NotFound($"User '{userGuid}' not found");

                var reservation = await _reservationService.GetReservationAsync(user.Id, reservationGuid);
                if (reservation == null)
                    return NotFound($"Reservation {reservationGuid} for user '{userGuid}' not found");

                if (reservation.ReservationDate?.Min(x => x.Date) <= _reservationService.GetRoomDateTimeNow().Date)
                    return Conflict("Can't modify an old reservation");

                await _reservationService.RemoveReservationAsync(reservation);

                _logger.LogInformation($"Finish removing reservation {reservationGuid} for user {userGuid}");
                return Ok($"Removed reservation {reservationGuid}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reservations for user '{userGuid}'");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }


        // For v1, if room not found, select default room
        private async Task<Room> GetRoom(int roomId) => await _roomService.GetByIdAsync(roomId) ?? await _roomService.GetByIdAsync(Constants.DEFAULT_ROOM_ID);

        private ObjectResult ValidateReservationDates(ReservationRequestDto reservationDto)
        {
            if (reservationDto.StartDate.Date > reservationDto.EndDate.Date)
                return BadRequest("StartDate greater than EndDate");

            if ((reservationDto.StartDate.Date - _reservationService.GetRoomDateTimeNow().Date).TotalDays < Constants.DAYS_AFTER_BOOKING)
                return BadRequest($"The reservation must start at least {Constants.DAYS_AFTER_BOOKING} day after the booking date");

            if ((reservationDto.EndDate.Date - reservationDto.StartDate.Date).TotalDays >= Constants.MAXIMUM_LENGTH_OF_STAY)
                return BadRequest($"Stay must be less or equal to {Constants.MAXIMUM_LENGTH_OF_STAY} days");

            if ((reservationDto.EndDate.Date - _reservationService.GetDateFromNow(Constants.DAYS_IN_ADVANCE).Date).TotalDays > 1)
                return BadRequest($"Can't reserve more than {Constants.DAYS_IN_ADVANCE} days in advance");

            return Ok(reservationDto);
        }

    }
}

