using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class LoanTypeService : ILoanTypeService
{
    private readonly MyAppDbContext context;
    public LoanTypeService(MyAppDbContext context)
    {
        this.context = context;
    }
    public async Task<List<LoanTypeModel>> GetAllLoanTypeAsync()
    {
        var loanTypes = await context.DbLoanType.ToListAsync();
        if (loanTypes.Count == 0)
        {
            return null;
        }
        return loanTypes;
    }

    public async Task<bool> AddLoanTypeAsync(LoanTypeModel loanType)
    {
        try
        {
            context.DbLoanType.Add(loanType);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteLoanTypeAsync(int loanTypeId)
    {
        try
        {
            var loanTypeToDelete = await context.DbLoanType.FirstAsync(l => l.LoanTypeId == loanTypeId);
            context.DbLoanType.Remove(loanTypeToDelete);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }


}