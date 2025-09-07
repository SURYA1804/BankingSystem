using DTO;

namespace interfaces;

public interface IManagerService
{
    Task<bool> CreateStaffAsync(RegisterDTO registerDTO);
    Task<List<UserDTO>> GetAllStaffAsync();
    Task<List<UserActivityDto>> GetUserActivityAsync();
}