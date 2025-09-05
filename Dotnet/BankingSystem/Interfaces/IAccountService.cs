using DTO;
using Model;

namespace interfaces;

public interface IAccountService
{
    Task<string> CreateAccountAsync(AccountCreationDTO accountCreationDTO);
    Task<string> RequestAccountTypeChangeAsync(long accountNumber, string newAccountType, int userId);
    Task<string> CloseAccountAsync(long accountNumber);
    Task<List<AccountDTO>> GetAccountsByUserIdAsync(int userId);
    Task<List<AccountDTO>> GetAllAccountsAsync();
    
}