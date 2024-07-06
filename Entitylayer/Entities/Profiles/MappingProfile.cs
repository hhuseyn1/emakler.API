using AutoMapper;
using DTO.Building;
using EntityLayer.Entities;

namespace EMakler.PROAPI.Entities.Profiles;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Building, BuildingDTO>().ReverseMap();

        CreateMap<BuildingPost, BuildingPostDTO>()
            .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.BuildingId))  // Map BuildingId
            .ReverseMap()
            .ForMember(dest => dest.BuildingId, opt => opt.MapFrom(src => src.BuildingId))  // Map BuildingId
            .ForMember(dest => dest.Building, opt => opt.Ignore());  // Ignore the Building property
    }
}

