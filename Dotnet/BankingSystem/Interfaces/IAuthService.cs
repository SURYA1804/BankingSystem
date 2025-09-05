using DTO;
using Model;

namespace interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtToken(UserDTO user);
}
