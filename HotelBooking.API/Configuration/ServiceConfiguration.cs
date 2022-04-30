using HotelBooking.DataAccess.EF.Repositories;
using HotelBooking.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooking.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static void MapServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        }
    }
}
