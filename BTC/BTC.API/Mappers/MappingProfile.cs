using AutoMapper;
using BTC.API.Models;
using BTC.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTC.API.Mappers
{
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SigningupSuccessResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => e.Id.ToString()));

            CreateMap<User, UserValidationSuccessResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => e.Id.ToString()));
        }
    }
}
