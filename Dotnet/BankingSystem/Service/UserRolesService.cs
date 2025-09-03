using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class UserRolesService : IUserRolesService
{
    private readonly MyAppDbContext context;

    public UserRolesService(MyAppDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AddRolesAsync(MasterRoles masterRoles)
    {
        try
        {
            await context.DbRoles.AddAsync(masterRoles);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

   
    public async Task<List<MasterRoles>> GetAllRolesAsync()
    {
        try
        {
            return await context.DbRoles.ToListAsync();
        }
        catch (Exception)
        {
            return new List<MasterRoles>(); 
        }
    }

    public async Task<bool> RemoveRolesAsync(string roleName)
    {
        try
        {
            var roleToDelete = await context.DbRoles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());

            if (roleToDelete == null)
                return false;

            context.DbRoles.Remove(roleToDelete);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
