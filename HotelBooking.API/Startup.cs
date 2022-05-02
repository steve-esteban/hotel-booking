using System;
using HotelBooking.API.Configuration;
using HotelBooking.DataAccess.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HotelBooking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("HotelBookingDatabase");
            services.AddDbContextPool<HotelBookingContext>(options => options.UseSqlServer(connection));

            ServiceConfiguration.MapServices(services);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();
            const string groupName = "v1";
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Hotel Booking {groupName}",
                    Version = groupName,
                    Contact = new OpenApiContact() { Name = "Source code", Url = new Uri("https://github.com/steve-esteban/hotel-booking") },
                    Description = "Hotel Booking app",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
