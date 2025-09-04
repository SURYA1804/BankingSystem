using Model;

namespace interfaces;

public interface IStaffService
{
    Task<string> ReviewAccountTypeChangeAsync(int ticketId, int staffId, int action);
    Task<List<AccountUpdateTicket>> GetAllAccountUpdateTickesAsync();
}