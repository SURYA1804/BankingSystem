using DTO;
using Model.DTOs;

namespace interfaces;

public interface ITransactionService
{
    Task<string> MakeTransactionAsync(MakeTransactionDTO makeTransactionDTO);

    Task<List<TransactionDTO>> GetAllTransactionsToApproveAsync();

    Task<string> ApproveTransactionAsync(int transactionId,string reason, int staffId, bool isApproved);

    Task<List<TransactionDTO>> GetAllTransactionByAccountAsync(long AccountNumber);
    
}