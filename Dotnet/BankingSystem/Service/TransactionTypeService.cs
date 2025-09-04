using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class TransactionTypeService : ITransactionTypeService
{
    private readonly MyAppDbContext context;

    public TransactionTypeService(MyAppDbContext context)
    {
        this.context = context;
    }

    public async Task<List<MasterTransactionType>> GetAllTransactionTypesAsync()
    {
        return await context.DbTransactionTypes.ToListAsync();
    }

    public async Task<string> AddTransactionTypeAsync(string transactionType)
    {
        try
        {
            var exists = await context.DbTransactionTypes
                .AnyAsync(t => t.TransactionType.ToLower() == transactionType.ToLower());

            if (exists)
                return "Transaction type already exists.";

            var type = new MasterTransactionType
            {
                TransactionType = transactionType
            };

            await context.DbTransactionTypes.AddAsync(type);
            await context.SaveChangesAsync();
            return "Transaction type added successfully.";
        }
        catch (Exception ex)
        {
            return $"Failed to add transaction type: {ex.Message}";
        }
    }

    public async Task<string> DeleteTransactionTypeAsync(int id)
    {
        try
        {
            var type = await context.DbTransactionTypes.FindAsync(id);
            if (type == null)
                return "Transaction type not found.";

            context.DbTransactionTypes.Remove(type);
            await context.SaveChangesAsync();
            return "Transaction type deleted successfully.";
        }
        catch (Exception ex)
        {
            return $"Failed to delete transaction type: {ex.Message}";
        }
    }
}
