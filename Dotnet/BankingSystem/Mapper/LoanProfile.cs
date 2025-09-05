using AutoMapper;
using Model;
using Model.DTOs;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<LoanModel, LoanDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User!.UserName))
            .ForMember(dest => dest.IsEmployed, opt => opt.MapFrom(src => src.User!.IsEmployed))
            .ForMember(dest => dest.CustomerType,opt=> opt.MapFrom(src => src.User!.CustomerType!.CustomerType))
            .ForMember(dest => dest.LoanTypeName, opt => opt.MapFrom(src => src.LoanType!.LoanTypeName));
    }
}
