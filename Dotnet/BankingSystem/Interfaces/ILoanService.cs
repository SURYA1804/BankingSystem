using DTO;
using Model;

namespace Interfaces;

public interface ILoanService
{
    Task<List<LoanModel>> GetAllLoansToApproveAsync();
    Task<List<LoanModel>> GetAllLoansAsync();

    Task<string> ApproveLoanAsync(int loanId, int staffId, bool isApproved);
    Task<string> CreateLoanRequestAsync(CreateLoanDTO createLoanDTO);
}