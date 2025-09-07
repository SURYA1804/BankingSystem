using DTO;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace Controllers;

[ApiController]
[Route("api/v1/CustomerSupport")]
public class CustomerSupportController : ControllerBase
{
    private readonly ICustomerSupportService customerSupportService;

    public CustomerSupportController(ICustomerSupportService customerSupportService)
    {
        this.customerSupportService = customerSupportService;
    }
    
    [Authorize]
    [HttpPost("CreateQuery")]
    public async Task<IActionResult> CreateQuery(RaiseTicketDTO raiseTicketDTO)
    {
        var result = await customerSupportService.CreateQueryAsync(raiseTicketDTO);
        if (result)
        {
            return Ok("Ticket Created");
        }
        return BadRequest("Not Created");
    }

    [Authorize]
    [HttpPost("AddComment")]
    public async Task<IActionResult> AddComment(AddCommentsDTO addCommentsDTO)
    {
        var result = await customerSupportService.AddCommentAsync(addCommentsDTO);
        if (result == null)
            return NotFound("Query not found.");
        return Ok(result);
    }

    [Authorize(Roles ="staff")]
    [HttpGet("GetAllPendingQueries")]
    public async Task<IActionResult> GetAllPendingQueries()
    {
        var result = await customerSupportService.GetAllPendingQueriesAsync();
        if (result == null || result.Count == 0)
        {
            return NotFound("No Queries Left");
        }
        return Ok(result);
    }

    [Authorize]
    [HttpGet("GetAllQueriesByUser")]
    public async Task<IActionResult> GetAllQueriesByUser(int UserId)
    {
        var result = await customerSupportService.GetAllQueriesByUserAsync(UserId);
        if (result == null || result.Count == 0)
        {
            return NotFound("No Queries Left");
        }
        return Ok(result);
    }

    [Authorize]
    [HttpPost("MarkQueryClosed")]
    public async Task<IActionResult> SolveQuery(int queryId, int staffId)
    {
        var success = await customerSupportService.MarkAsSolvedAsync(queryId, staffId);
        if (!success)
            return NotFound("Query not found.");
        return Ok("Ticket marked as solved.");
    }


}
