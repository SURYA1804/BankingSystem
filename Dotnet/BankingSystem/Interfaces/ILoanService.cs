using DTO;
using Model;
using Model.DTOs;

namespace Interfaces;

public interface ILoanService
{
    Task<List<LoanDTO>> GetAllLoansToApproveAsync();
    Task<List<LoanDTO>> GetAllLoansAsync();

    Task<string> ApproveLoanAsync(int loanId, int staffId, bool isApproved,string reason);
    Task<string> CreateLoanRequestAsync(CreateLoanDTO createLoanDTO);
    Task<List<LoanDTO>> GetAllLoansByUserAsync(int userId);
}