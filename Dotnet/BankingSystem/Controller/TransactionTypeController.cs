using interfaces;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers;

[ApiController]
[Route("api/v1/TranscationType")]
public class TransactionTypeController : ControllerBase
{
    private readonly ITransactionTypeService transactionTypeService;

    public TransactionTypeController(ITransactionTypeService transactionTypeService)
    {
        this.transactionTypeService = transactionTypeService;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await transactionTypeService.GetAllTransactionTypesAsync();
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] string transactionType)
    {
        var result = await transactionTypeService.AddTransactionTypeAsync(transactionType);
        if (result.Contains("successfully"))
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await transactionTypeService.DeleteTransactionTypeAsync(id);
        if (result.Contains("successfully"))
            return Ok(result);

        return BadRequest(result);
    }
}
