using DTO;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace interfaces;

public interface IUserService
{
    Task<bool> RegisterCustomerAsync(RegisterDTO registerDTO);
    Task<UsersModel> LoginAsync(LoginDTO loginDTO);
    Task<string> VerifyOtpAsync(string email, int otp);
    Task<bool> GetOTPAsync(string email);
    
}