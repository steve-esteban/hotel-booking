using AutoMapper;
using HotelBooking.API.Models;
using HotelBooking.Model;

namespace HotelBooking.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
