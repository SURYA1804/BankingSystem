using DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace interfaces;

public interface IUserService
{
    Task<bool> RegisterCustomerAsync(RegisterDTO registerDTO);
    Task<UserDTO> LoginAsync(LoginDTO loginDTO);
    Task<string> VerifyOtpAsync(string email, int otp);
    Task<bool> GetOTPAsync(string email);
    Task<UsersModel?> GetUserByIdAsync(int userId);
    Task<bool> UpdateUserAsync(int userId, JsonPatchDocument<UsersModel> patchDoc);
    Task<bool> CheckPasswordAsync(int userId, string password);
    
}