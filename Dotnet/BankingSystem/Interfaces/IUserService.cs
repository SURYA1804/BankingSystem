using DTO;
using Microsoft.AspNetCore.Mvc;

namespace interfaces;

public interface IUserService
{
    Task<bool> RegisterCustomerAsync(RegisterDTO registerDTO);
    // Task<UsersModel> LoginAsync(LoginDTO loginDTO);
    Task<bool> VerifyOtpAsync(string email, int otp);
}