using DTO;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class LoanService:ILoanService
{
    private readonly MyAppDbContext context;
    public LoanService(MyAppDbContext context)
    {
        this.context = context;
    }

    public async Task<string> CreateLoanRequestAsync(CreateLoanDTO createLoanDTO)
    {
        try
        {
            var userExists = await context.DbUsers.AnyAsync(u => u.UserId == createLoanDTO.UserId);
            if (!userExists)
                return "User not found.";

            var loanTypeExists = await context.DbLoanType.AnyAsync(l => l.LoanTypeId == createLoanDTO.LoanTypeId);
            if (!loanTypeExists)
                return "Invalid loan type.";

            var loan = new LoanModel
            {
                UserId = createLoanDTO.UserId,
                LoanTypeId = createLoanDTO.LoanTypeId,
                CurrentSalaryInLPA = createLoanDTO.CurrentSalaryInLPA,
                CreatedAt = IndianTime.GetIndianTime(),
                IsApproved = false
            };

            await context.DbLoan.AddAsync(loan);
            await context.SaveChangesAsync();

            return "Loan request created successfully. Awaiting approval.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LoanService] CreateLoanRequest failed: {ex.Message}");
            return "Failed to create loan request.";
        }
    }
    public async Task<string> ApproveLoanAsync(int loanId, int staffId, bool isApproved)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var loan = await context.DbLoan
                .Include(l => l.User)
                .Include(l => l.LoanType)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
                return "Loan not found.";

            if (loan.IsApproved)
                return "Loan already processed.";

            loan.IsApproved = isApproved;
            loan.ApprovedBy = staffId;
            loan.ApprovedAt = IndianTime.GetIndianTime();

            context.DbLoan.Update(loan);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return isApproved ? "Loan approved successfully." : "Loan rejected.";
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return $"Approval failed: {ex.Message}";
        }
    }

    public async Task<List<LoanModel>> GetAllLoansAsync()
    {
        return await context.DbLoan
            .Include(l => l.User)
            .Include(l => l.LoanType)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }
    public async Task<List<LoanModel>> GetAllLoansToApproveAsync()
    {
        return await context.DbLoan
            .Include(l => l.User)
            .Include(l => l.LoanType)
            .Where(l => !l.IsApproved)
            .OrderBy(l => l.CreatedAt)
            .ToListAsync();
    }
}