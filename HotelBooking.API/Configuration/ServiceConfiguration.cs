using HotelBooking.DataAccess.EF.Repositories;
using HotelBooking.Services;
using HotelBooking.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static void MapServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IReservationService, ReservationService>();

            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        }
    }
}
