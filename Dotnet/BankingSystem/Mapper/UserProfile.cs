using AutoMapper;
using Model;
using DTO;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UsersModel, UserDTO>()
            .ForMember(dest => dest.CustomerTypeName, opt => opt.MapFrom(src => src.CustomerType != null ? src.CustomerType.CustomerType : null))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null));
    }
}
