using AutoMapper;
using BTC.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BTC.Services.Mappers
{
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => Guid.NewGuid()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(e => DateTime.Now))
                .ReverseMap();
        }
    }
}
