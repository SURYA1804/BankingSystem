using interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace Controllers;

[ApiController]
[Route("api/v1/AccountType")]
public class AccountTypeController : ControllerBase
{
    private readonly IAccountTypeService accountTypeService;

    public AccountTypeController(IAccountTypeService accountTypeService)
    {
        this.accountTypeService = accountTypeService;
    }

    [HttpGet("GetAllAccountType")]
    public async Task<IActionResult> GetAll()
    {
        var result = await accountTypeService.GetAllAccountTypeAsync();
        if (result != null && result.Any())
            return Ok(result);
        return NotFound(new { message = "No account types found" });
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] MasterAccountTypeModel accountTypeModel)
    {
        var success = await accountTypeService.AddAccountTypeAsync(accountTypeModel);
        if (success)
            return Ok(new { message = "Account type added successfully" });
        return BadRequest(new { message = "Failed to add account type" });
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> Remove(string accountType)
    {
        var success = await accountTypeService.RemoveAccountTypeAsync(accountType);
        if (success)
            return Ok(new { message = "Account type removed successfully" });
        return NotFound(new { message = "Account type not found" });
    }
}
