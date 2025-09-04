using DTO;
using Model;

namespace interfaces;

public interface IAccountService
{
    Task<string> CreateAccountAsync(AccountCreationDTO accountCreationDTO);
    Task<string> RequestAccountTypeChangeAsync(long accountNumber, string newAccountType, int userId);
    Task<string> CloseAccountAsync(long accountNumber);
    Task<List<AccountModel>> GetAccountsByUserIdAsync(int userId);
    Task<List<AccountModel>> GetAllAccountsAsync();
    
}