using AutoMapper;
using Model;
using DTO;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<AccountModel, AccountDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.AccountTypeId, opt => opt.MapFrom(src => src.AccountTypeId))
            .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountType.AccountType))
            .ForMember(dest => dest.LastTransactionAt, opt => opt.MapFrom(src => src.LastTransactionAt))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.IsAccountClosed, opt => opt.MapFrom(src => src.IsAccountClosed));
    }
}
