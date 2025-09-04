using DTO;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/v1/Loan")]
public class LoanController : ControllerBase
{
    private readonly ILoanService loanService;

    public LoanController(ILoanService loanService)
    {
        this.loanService = loanService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateLoan([FromBody] CreateLoanDTO createLoanDTO)
    {
        var result = await loanService.CreateLoanRequestAsync(createLoanDTO);
        if (result.Contains("successfully"))
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("approve")]
    public async Task<IActionResult> ApproveLoan([FromQuery] int loanId, [FromQuery] int staffId, [FromQuery] bool isApproved)
    {
        var result = await loanService.ApproveLoanAsync(loanId, staffId, isApproved);
        if (result.Contains("failed") || result.Contains("not found"))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("GetAllLoans")]
    public async Task<IActionResult> GetAllLoans()
    {
        var loans = await loanService.GetAllLoansAsync();
         if (loans == null || !loans.Any())
            return NotFound("No  loan requests found.");

        return Ok(loans);
    }

    [HttpGet("pending-approvals")]
    public async Task<IActionResult> GetAllLoansToApprove()
    {
        var loans = await loanService.GetAllLoansToApproveAsync();
        if (loans == null || !loans.Any())
            return NotFound("No pending loan requests found.");

        return Ok(loans);
    }
}
