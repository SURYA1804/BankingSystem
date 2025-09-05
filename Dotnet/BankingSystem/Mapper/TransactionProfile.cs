using AutoMapper;
using Model;
using Model.DTOs;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<TransactionModel, TransactionDTO>()
            .ForMember(dest => dest.TransactionType,
                       opt => opt.MapFrom(src => src.TransactionType.TransactionType))
            .ForMember(dest => dest.FromAccount,
                       opt => opt.MapFrom(src => src.FromAccount.AccountNumber))
            .ForMember(dest => dest.FromUser,
                       opt => opt.MapFrom(src => src.FromAccount.User.Name))
            .ForMember(dest => dest.FromEmail,
                       opt => opt.MapFrom(src => src.FromAccount.User.Email))
            .ForMember(dest => dest.ToAccount,
                       opt => opt.MapFrom(src => src.ToAccount.AccountNumber))
            .ForMember(dest => dest.ToUser,
                       opt => opt.MapFrom(src => src.ToAccount.User.Name))
            .ForMember(dest => dest.ToEmail,
                       opt => opt.MapFrom(src => src.ToAccount.User.Email));

    }
}
