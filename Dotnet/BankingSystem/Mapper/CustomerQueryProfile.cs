using AutoMapper;
using Model;

public class CustomerQueryProfile : Profile
{
    public CustomerQueryProfile()
    {
        CreateMap<CustomerQueryModel, CustomerQueryDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
            .ForMember(dest => dest.QueryType, opt => opt.MapFrom(src => src.queryType.QueryType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.QueryStatus.StatusName))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.QueryPriority.PriorityName))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.QueryComments));

        CreateMap<QueryComments, QueryCommentsDTO>()
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.comments));
    }
}
