using DTO;
using interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers;

[ApiController]
[Route("api/v1/Account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService accountService;

    public AccountController(IAccountService accountService)
    {
        this.accountService = accountService;
    }
    
    [Authorize]
    [HttpPost("CreateAccount")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreationDTO accountCreationDTO)
    {
        var result = await accountService.CreateAccountAsync(accountCreationDTO);

        if (result == "Success")
            return Ok(new { message = "Account created successfully" });

        return BadRequest(new { message = result });
    }

    [Authorize]
    [HttpPost("request-change")]
    public async Task<IActionResult> RequestAccountTypeChange(long accountNumber, string newAccountType, int userId)
    {
        var result = await accountService.RequestAccountTypeChangeAsync(accountNumber, newAccountType, userId);
        if (result == "Account not found." || result == "Invalid account type." || result == "Cannot update a closed account.")
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("CloseAccount")]
    public async Task<IActionResult> CloseAccount(long accountNumber)
    {
        var result = await accountService.CloseAccountAsync(accountNumber);
        if (result == "Account closed successfully.")
            return Ok(new { message = result });

        return BadRequest(new { message = result });
    }

    [Authorize]
    [HttpGet("AllAccount")]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await accountService.GetAllAccountsAsync();
        return Ok(accounts);
    }

    [Authorize]
    [HttpGet("AccountByUser")]
    public async Task<IActionResult> GetAccountsByUserId(int userId)
    {
        var accounts = await accountService.GetAccountsByUserIdAsync(userId);
        if (accounts == null || accounts.Count == 0)
            return NotFound(new { message = "No accounts found for this user." });

        return Ok(accounts);
    }
}
