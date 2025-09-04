using Model;

namespace interfaces;

public interface ILoanTypeService
{
    Task<List<LoanTypeModel>> GetAllLoanTypeAsync();
    Task<bool> AddLoanTypeAsync(LoanTypeModel loanType);
    Task<bool> DeleteLoanTypeAsync(int loanTypeId);
    
}