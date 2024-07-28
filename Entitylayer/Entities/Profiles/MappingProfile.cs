using AutoMapper;
using DTO.BuildingPost;
using DTO.User;
using EntityLayer.Entities;

namespace EMakler.PROAPI.Entities.Profiles;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Email, src => src.MapFrom(u => u.Email))
            .ForMember(dest => dest.IsVerified, src => src.MapFrom(u => u.IsVerified))
            .ForMember(dest => dest.ContactNumber, src => src.MapFrom(u => u.ContactNumber));


        CreateMap<BuildingPost, BuildingPostDto>()
            .ForMember(dest => dest.IsActive, src => src.MapFrom(u => u.IsActive))
            .ForMember(dest => dest.Building, src => src.MapFrom(u => u.Building));

    }
}

