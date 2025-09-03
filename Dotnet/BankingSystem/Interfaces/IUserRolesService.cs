using Model;

namespace interfaces;

public interface IUserRolesService
{
    Task<List<MasterRoles>> GetAllRolesAsync();  
    Task<bool> AddRolesAsync(MasterRoles masterRoles);
    Task<bool> RemoveRolesAsync(string roleName);
}
