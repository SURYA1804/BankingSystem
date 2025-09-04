using DTO;

namespace interfaces;

public interface IManagerService  
{
    Task<bool> CreateStaffAsync(RegisterDTO registerDTO);
}