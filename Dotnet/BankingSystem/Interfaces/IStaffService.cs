using Model;
using Model.DTOs;

namespace interfaces;

public interface IStaffService
{
    Task<string> ReviewAccountTypeChangeAsync(int ticketId, int staffId, int action);
    Task<List<AccountUpdateTicketDTO>> GetAllAccountUpdateTickesAsync();
    Task<List<AccountUpdateTicketDTO>> GetALlPendingAccountUpdateTicketAsync();
}