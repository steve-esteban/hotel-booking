using HotelBooking.DataAccess.EF;
using HotelBooking.Model;
using HotelBooking.API.Models;
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
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HotelBookingContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger
            , HotelBookingContext context
            )
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            try
            {
                var products = await _context.Hotel.FindAsync(1);
                _logger.LogInformation(products.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("test")]
        public async Task<string> GetNameAsync()
        {
            var products = new Hotel();
            try
            {
                products = await _context.Hotel.FindAsync(1);
                _logger.LogInformation(products.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }

            return products?.Name;
        }
    }
}
