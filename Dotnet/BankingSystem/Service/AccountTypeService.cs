using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class AccountTypeService : IAccountTypeService
{
    private readonly MyAppDbContext context;
    public AccountTypeService(MyAppDbContext context)
    {
        this.context = context;
    }
    public async Task<List<MasterAccountTypeModel>?> GetAllAccountTypeAsync()
    {
        var accountType = await context.DbAccountType.ToListAsync();
        if (accountType == null)
        {
            return null;
        }
        return accountType;
    }

    public async Task<bool> AddAccountTypeAsync(MasterAccountTypeModel accountTypeModel)
    {
        try
        {
            await context.DbAccountType.AddAsync(accountTypeModel);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public async Task<bool> RemoveAccountTypeAsync(string AccountType)
    {
        try
        {
            var accountType = await context.DbAccountType.FirstAsync(m=> m.AccountType.ToLower() == AccountType.ToLower());
            context.DbAccountType.Remove(accountType);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}