using Model;

namespace interfaces;

public interface IAuthService
{
    Task<string> GenerateJwtToken(UsersModel user);
}
