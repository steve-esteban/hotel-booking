using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using HotelBooking.DataAccess.EF;
using HotelBooking.DataAccess.EF.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using HotelBooking.API.Models;
using HotelBooking.Model;
using Microsoft.AspNetCore.Http;


namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly HotelBookingContext _context;

        public BookingController(ILogger<BookingController> logger, HotelBookingContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet("{roomId:int}/Availability")]
        public async Task<IActionResult> GetAvailabilityAsync([FromRoute] int roomId)
        {
            var room = await GetRoom(roomId);

            // var reservations = await _context.Reservation.Where(x => x.IsActive && x.RoomId == room.Id).ToListAsync();
            return Ok();
        }

        [HttpPost("{roomId:int}/MakeReservation")]
        public async Task<IActionResult> MakeReservation([FromBody][Bind] ReservationDto reservationDto)
        {
            _logger.LogInformation($"Making reservation: {Newtonsoft.Json.JsonConvert.SerializeObject(reservationDto)}");
            var room = await GetRoom(reservationDto.RoomId);

            var availableDates = await GetAvailableDatesAsync(room.Id);
            var validation = ValidateReservation(reservationDto);
            if (validation.StatusCode != StatusCodes.Status200OK)
            {
                _logger.LogError("Error making reservation", validation.Value);
                return validation;
            }
            return Ok();
        }

        private async Task<Room> GetRoom(int roomId)
        {
            // For v1.1, if room not found, select default value
            return await _context.Room.FirstOrDefaultAsync(x => x.Id == roomId) ??
                 await _context.Room.FirstOrDefaultAsync(x => x.Id == Constants.ROOM_ID);
        }

        private ObjectResult ValidateReservation(ReservationDto reservationDto)
        {
            if (reservationDto.StartDate.Date > reservationDto.EndDate.Date)
            {
                return BadRequest("StartDate greater than EndDate");
            }

            if ((reservationDto.EndDate.Date - reservationDto.StartDate.Date).TotalDays > 3)
            {
                return Conflict("Stay must be less or equal to 3 days");
            }

            if ((GetRoomDateTimeNow().Date - reservationDto.StartDate.Date).TotalDays < 1)
            {
                return Conflict("The reservation must start at least one day after the booking date");
            }

            return Ok(reservationDto);
        }

        private async Task<List<DateTime>> GetAvailableDatesAsync(int roomId)
        {
            var roomDateTimeNow = GetRoomDateTimeNow();
            var dates = Enumerable.Range(0, 30).Select(offset => roomDateTimeNow.AddDays(offset)).ToList();
            //var reservations = await _context.Reservation.Where(x => x.IsActive && x.RoomId == roomId).ToListAsync();
            //foreach (var reservation in reservations)
            //{
            //    var bookedDates = Enumerable.Range(0, reservation.EndDate.Subtract(reservation.StartDate).Days)
            //        .Select(offset => reservation.StartDate.AddDays(offset));
            //    foreach (var bookedDate in bookedDates)
            //    {
            //        dates = dates.Where(x => x.Date != bookedDate.Date).ToList();
            //    }
            //}

            return dates;
        }

        private DateTime GetRoomDateTimeNow()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        }

    }
}
