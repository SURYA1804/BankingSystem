using Microsoft.AspNetCore.Mvc;
using Service;
using Model;
using interfaces;
using DTO;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpPost("MakeTransaction")]
        public async Task<IActionResult> MakeTransaction(MakeTransactionDTO makeTransactionDTO)
        {
            var result = await transactionService.MakeTransactionAsync(makeTransactionDTO);
            if (result.Contains("failed") || result.Contains("Invalid") || result.Contains("Insufficient"))
                return BadRequest(result);

            return Ok(result);
        }
        
        [Authorize(Roles = "staff")]
        [HttpGet("GetAllTransactionsToApprove")]
        public async Task<IActionResult> GetAllTransactionsToApprove()
        {
            var transactions = await transactionService.GetAllTransactionsToApproveAsync();
            return Ok(transactions);
        }

        [Authorize]
        [HttpGet("GetAllTransactionsByAccount")]
        public async Task<IActionResult> GetAllTransactionsByAccount(long AccountNumber)
        {
            var transactions = await transactionService.GetAllTransactionByAccountAsync(AccountNumber);
            return Ok(transactions);
        }

    [Authorize(Roles="staff")]
    [HttpPost("ApproveTransaction")]
    public async Task<IActionResult> ApproveTransaction(int transactionId,string reason, int staffId, bool isApproved)
    {
        var result = await transactionService.ApproveTransactionAsync(transactionId, reason,staffId, isApproved);

        if (result == "Transaction rejected.")
            {
                return Ok(result);
            }
        if (result !="Transaction approved successfully.")
            return BadRequest(result);

        return Ok(result);
    }

    }
}
