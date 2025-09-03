namespace Service;

using System.Collections.Generic;
using System.Threading.Tasks;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

public class CustomerTypeService : ICustomerTypeService
{
    private readonly MyAppDbContext context;

    public CustomerTypeService(MyAppDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AddCustomerType(MasterCustomerType customerType)
    {
        try
        {
            await context.DbCustomerTypes.AddAsync(customerType);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCustomerType(string customerType)
    {
        try
        {
            var customerTypeToDelete = await context.DbCustomerTypes
                .FirstOrDefaultAsync(c => c.CustomerType.ToLower() == customerType.ToLower());

            if (customerTypeToDelete == null)
                return false;

            context.DbCustomerTypes.Remove(customerTypeToDelete);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<MasterCustomerType>> GetAllCustomerType()
    {
        try
        {
            return await context.DbCustomerTypes.ToListAsync();
        }
        catch (Exception)
        {
            return new List<MasterCustomerType>();
        }
    }
}
