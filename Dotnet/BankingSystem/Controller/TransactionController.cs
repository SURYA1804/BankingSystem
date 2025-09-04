using Microsoft.AspNetCore.Mvc;
using Service;
using Model;
using interfaces;
using DTO;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/Transcation")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpPost("MakeTransaction")]
        public async Task<IActionResult> MakeTransaction(MakeTransactionDTO makeTransactionDTO)
        {
            var result = await transactionService.MakeTransactionAsync(makeTransactionDTO);
            if (result.Contains("failed") || result.Contains("Invalid") || result.Contains("Insufficient"))
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("GetAllTransactionsToApprove")]
        public async Task<IActionResult> GetAllTransactionsToApprove()
        {
            var transactions = await transactionService.GetAllTransactionsToApproveAsync();
            return Ok(transactions);
        }


    [HttpPost("ApproveTransaction")]
    public async Task<IActionResult> ApproveTransaction(int transactionId, int staffId, bool isApproved)
    {
        var result = await transactionService.ApproveTransactionAsync(transactionId, staffId, isApproved);

        if (result.StartsWith("Approval failed") || result.Contains("not found") || result.Contains("rejected"))
            return BadRequest(result);

        return Ok(result);
    }

    }
}
