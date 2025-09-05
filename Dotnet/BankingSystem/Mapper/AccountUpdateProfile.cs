using AutoMapper;
using Model;
using Model.DTOs;

public class AccountUpdateTicketProfile : Profile
{
    public AccountUpdateTicketProfile()
    {
        CreateMap<AccountUpdateTicket, AccountUpdateTicketDTO>();
    }
}
