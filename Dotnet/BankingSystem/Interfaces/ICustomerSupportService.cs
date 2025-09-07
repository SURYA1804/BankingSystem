using DTO;
using Model;

namespace Interfaces;

public interface ICustomerSupportService
{
    Task<bool> CreateQueryAsync(RaiseTicketDTO raiseTicketDTO);
    Task<QueryComments> AddCommentAsync(AddCommentsDTO addCommentsDTO);
    Task<List<CustomerQueryDTO>> GetAllPendingQueriesAsync();
    Task<bool> MarkAsSolvedAsync(int queryId, int staffId);
    Task<List<CustomerQueryDTO>> GetAllQueriesByUserAsync(int UserId);
}