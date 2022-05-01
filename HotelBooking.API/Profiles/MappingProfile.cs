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

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.RoomId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Room, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Hotel, act => act.MapFrom(src => src.Hotel.Name));

            CreateMap<Room, AvailabilityDto>()
                .ForMember(dest => dest.RoomId, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Room, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Hotel, act => act.MapFrom(src => src.Hotel.Name));

        }
    }
}
