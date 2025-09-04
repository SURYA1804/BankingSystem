using Model;

namespace interfaces;

public interface ITransactionTypeService
{
    Task<List<MasterTransactionType>> GetAllTransactionTypesAsync();
    Task<string> AddTransactionTypeAsync(string transactionType);
    Task<string> DeleteTransactionTypeAsync(int id);
}