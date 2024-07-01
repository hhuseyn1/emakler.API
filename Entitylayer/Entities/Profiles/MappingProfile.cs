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
            .ForMember(dest => dest.Building, opt => opt.MapFrom(src => src.Building))
            .ReverseMap();
    }
}
