using DTO;
using interfaces;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers;

[ApiController]
[Route("api/v1/Staff")]
public class StaffController : ControllerBase
{
    private readonly IStaffService staffService;

    public StaffController(IStaffService staffService)
    {
        this.staffService = staffService;
    }
    [HttpPost("ReviewAccountTypeChange")]
    public async Task<IActionResult> ReviewAccountTypeChange(int ticketId, int staffId, int action)
    {
        if (action != 1 && action != 2)
        {
            return BadRequest("Invalid action. Use 1 for approve, 2 for reject.");
        }

        var result = await staffService.ReviewAccountTypeChangeAsync(ticketId, staffId, action);

        if (result == "Ticket not found." || result == "Invalid ticket format." || result.StartsWith("Invalid account type"))
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("GetAllAccountTickets")]
    public async Task<IActionResult> GetAllTicket()
    {
        var ticket = await staffService.GetAllAccountUpdateTickesAsync();
        if (ticket == null)
        {
            NotFound("No Tickets");
        }
        return Ok(ticket);
    }

}