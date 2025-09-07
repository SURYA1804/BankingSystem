using AutoMapper;
using Model;

public class UserActivityProfile : Profile
{
    public UserActivityProfile()
    {
        CreateMap<AccountModel, AccountActivityDto>()
            .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountType != null ? src.AccountType.AccountType : string.Empty));

        CreateMap<UsersModel, UserActivityDto>()
            .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerType.CustomerType))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.AccountsList, opt => opt.MapFrom(src => src.Account!.Where(a => a.UserId == src.UserId)));
    }
}
