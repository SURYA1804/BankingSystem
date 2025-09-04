using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using interfaces;

namespace Controllers
{
    [ApiController]
    [Route("api/v1/LoanType")]
    public class LoanTypeController : ControllerBase
    {
        private readonly ILoanTypeService loanTypeService;

        public LoanTypeController(ILoanTypeService loanTypeService)
        {
            this.loanTypeService = loanTypeService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllLoanTypes()
        {
            var loanTypes = await loanTypeService.GetAllLoanTypeAsync();
            if (loanTypes == null || loanTypes.Count == 0)
                return NotFound("No loan types found.");

            return Ok(loanTypes);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLoanType([FromBody] LoanTypeModel loanType)
        {
            if (loanType == null || string.IsNullOrWhiteSpace(loanType.LoanTypeName))
                return BadRequest("Invalid loan type.");

            var result = await loanTypeService.AddLoanTypeAsync(loanType);
            if (result)
                return Ok("Loan type added successfully.");
            return StatusCode(500, "Failed to add loan type.");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLoanType(int id)
        {
            var result = await loanTypeService.DeleteLoanTypeAsync(id);
            if (result)
                return Ok("Loan type deleted successfully.");
            return StatusCode(500, "Failed to delete loan type.");
        }
    }
}
