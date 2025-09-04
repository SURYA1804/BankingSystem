using DTO;

namespace interfaces;

public interface ITransactionService
{
    Task<string> MakeTransactionAsync(MakeTransactionDTO makeTransactionDTO);

    Task<List<object>> GetAllTransactionsToApproveAsync();

    Task<string> ApproveTransactionAsync(int transactionId, int staffId, bool isApproved);
    
}