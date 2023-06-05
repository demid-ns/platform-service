using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class PlatformProfile: Profile
    {
        public PlatformProfile()
        {
            //source -> target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(source => source.Id));
        }
    }
}
