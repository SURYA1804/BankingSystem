using AutoMapper;
using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class AccountService : IAccountService
{
    private readonly MyAppDbContext context;
    private readonly IMapper mapper;
    public AccountService(MyAppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public async Task<string> CreateAccountAsync(AccountCreationDTO accountCreationDTO)
    {
        try
        {
            var userExists = await context.DbUsers.AnyAsync(u => u.UserId == accountCreationDTO.UserId);
            if (!userExists)
            {
                return "User not found.";
            }

            var accountType = await context.DbAccountType
                .FirstOrDefaultAsync(a => a.AccountType.ToLower() == accountCreationDTO.AccountType.ToLower());

            if (accountType == null)
            {
                return "Invalid account type.";
            }

            bool alreadyExists = await context.DbAccount.AnyAsync(a =>
                a.UserId == accountCreationDTO.UserId &&
                a.AccountTypeId == accountType.AccountTypeID &&
                a.IsActive && a.IsAccountClosed);

            if (alreadyExists)
            {
                return "User already has an active account of this type.";
            }

            var account = new AccountModel
            {
                Balance = accountCreationDTO.OpeningBalance,
                UserId = accountCreationDTO.UserId,
                AccountTypeId = accountType.AccountTypeID,
                CreatedAt = IndianTime.GetIndianTime(),
                Currency = "INR",
                IsActive = true,
                IsAccountClosed = false
            };

            await context.DbAccount.AddAsync(account);
            await context.SaveChangesAsync();

            return "Success";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AccountService] CreateAccount failed: {ex.Message}");
            return "failed";
        }
    }

    public async Task<string> CloseAccountAsync(long accountNumber)
    {
        try
        {
            var account = await context.DbAccount.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                return "Account not found.";
            }

            if (account.IsAccountClosed)
            {
                return "Account is already closed.";
            }

            account.IsActive = false;
            account.IsAccountClosed = true;
            account.ClosedAt = IndianTime.GetIndianTime();

            context.DbAccount.Update(account);
            await context.SaveChangesAsync();

            return "Account closed successfully.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AccountService] CloseAccount failed: {ex.Message}");
            return "failed";
        }
    }
    public async Task<string> RequestAccountTypeChangeAsync(long accountNumber, string newAccountType, int userId)
    {
        var account = await context.DbAccount.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        if (account == null)
            return "Account not found.";

        if (account.IsAccountClosed)
            return "Cannot update a closed account.";

        var accountType = await context.DbAccountType
            .FirstOrDefaultAsync(a => a.AccountType.ToLower() == newAccountType.ToLower());

        if (accountType == null)
            return "Invalid account type.";

        var ticket = new AccountUpdateTicket
        {
            AccountNumber = accountNumber,

            RequestedChange = $"Change AccountType to {newAccountType}",
            RequestedBy = userId.ToString()
        };

        await context.DbAccountUpdateTickets.AddAsync(ticket);
        await context.SaveChangesAsync();

        return "Account update request submitted. Awaiting staff approval.";
    }
    public async Task<List<AccountDTO>> GetAllAccountsAsync()
    {
        var result = await context.DbAccount.Include(a => a.User).Include(a => a.AccountType).ToListAsync();
        return mapper.Map<List<AccountDTO>>(result);
    }

    public async Task<List<AccountDTO>> GetAccountsByUserIdAsync(int userId)
    {
        var result = await context.DbAccount
            .Where(a => a.UserId == userId)
            .Include(a => a.User)
            .Include(a => a.AccountType)
            .ToListAsync();
        return mapper.Map<List<AccountDTO>>(result);
            
    }


}