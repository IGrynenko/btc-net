using AutoMapper;
using BTC.Services.Models;
using System;

namespace BTC.Services.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => Guid.NewGuid()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(e => DateTime.Now));

            CreateMap<User, UserModel>();
        }
    }
}
