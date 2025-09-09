using AutoMapper;
using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTOs;
using MyDbContext;

namespace Service
{
    public class TransactionService : ITransactionService
    {
        private readonly MyAppDbContext context;
        private readonly IMapper mapper;

        public TransactionService(MyAppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<string> MakeTransactionAsync(MakeTransactionDTO makeTransactionDTO)
        {
            using var dbTransaction = await context.Database.BeginTransactionAsync();
            try
            {
                var fromAccount = await context.DbAccount.FirstOrDefaultAsync(a => a.AccountNumber == makeTransactionDTO.FromAccount);
                var toAccount = await context.DbAccount.FirstOrDefaultAsync(a => a.AccountNumber == makeTransactionDTO.ToAccount);

                if (fromAccount == null || toAccount == null)
                    return "Invalid account number(s).";

                if (fromAccount.IsAccountClosed || toAccount.IsAccountClosed)
                    return "One of the accounts is closed.";

                var transaction = new TransactionModel
                {
                    FromAccountNumber = makeTransactionDTO.FromAccount,
                    ToAccountNumber = makeTransactionDTO.ToAccount,
                    TransactionTypeID = makeTransactionDTO.TransactionTypeId,
                    TransactionDate = IndianTime.GetIndianTime(),
                    Amount = makeTransactionDTO.Amount
                };

                if (makeTransactionDTO.Amount > 10000 && makeTransactionDTO.TransactionTypeId == 3)
                {
                    transaction.IsVerificationRequired = true;
                    transaction.IsSuccess = false;
                    transaction.ErrorMessage = "Pending staff approval for high value transaction.";
                }
                else
                {
                    if (fromAccount.Balance < makeTransactionDTO.Amount)
                    {
                        transaction.IsSuccess = false;
                        transaction.ErrorMessage = "Insufficient balance.";
                    }
                    else
                    {
                        fromAccount.Balance -= (int)makeTransactionDTO.Amount;
                        toAccount.Balance += (int)makeTransactionDTO.Amount;
                        transaction.IsSuccess = true;
                        transaction.ErrorMessage = null;

                        fromAccount.LastTransactionAt = IndianTime.GetIndianTime();
                        toAccount.LastTransactionAt = IndianTime.GetIndianTime();

                        context.DbAccount.Update(fromAccount);
                        context.DbAccount.Update(toAccount);
                    }
                }

                await context.DbTransactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                await dbTransaction.CommitAsync();

                return transaction.IsSuccess ? "Transaction successful." : transaction.ErrorMessage ?? "Transaction pending.";
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                return $"Transaction failed: {ex.Message}";
            }
        }
        public async Task<List<TransactionDTO>> GetAllTransactionsToApproveAsync()
        {
            try
            {
                var transactions = await context.DbTransactions
                    .Where(t => t.IsVerificationRequired && !t.IsSuccess)
                    .Include(t => t.FromAccount).ThenInclude(a => a.User)
                    .Include(t => t.ToAccount).ThenInclude(a => a.User)
                    .Include(t => t.TransactionType)
                    .ToListAsync();

                return mapper.Map<List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                return  null;
            }
        }
        public async Task<List<TransactionDTO>> GetAllTransactionByAccountAsync(long AccountNumber)
        {
            try
            {
                var transactions = await context.DbTransactions
                    .Where(t => t.FromAccountNumber == AccountNumber || t.ToAccountNumber == AccountNumber)
                    .Include(t => t.FromAccount).ThenInclude(a => a.User)
                    .Include(t => t.ToAccount).ThenInclude(a => a.User)
                    .Include(t => t.TransactionType).OrderByDescending(m=>m.TransactionId)
                    .ToListAsync();

                return mapper.Map<List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                return  null;
            }
        }


        public async Task<string> ApproveTransactionAsync(int transactionId,string reason, int staffId, bool isApproved)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var txn = await context.DbTransactions
                    .Include(t => t.FromAccount)
                    .Include(t => t.ToAccount)
                    .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

                if (txn == null)
                    return "Transaction not found.";

                if (!txn.IsVerificationRequired)
                    return "Transaction does not require approval.";

                if (txn.IsSuccess)
                    return "Transaction already processed.";

                if (!isApproved)
                {
                    txn.IsVerificationRequired = false;
                    txn.IsSuccess = false;
                    txn.ErrorMessage = reason;
                }
                else
                {
                    if (txn.FromAccount.Balance < txn.Amount)
                        return "Insufficient balance.";

                    txn.FromAccount.Balance -= (int)txn.Amount;
                    txn.ToAccount.Balance += (int)txn.Amount;

                    txn.FromAccount.LastTransactionAt = IndianTime.GetIndianTime();
                    txn.ToAccount.LastTransactionAt = IndianTime.GetIndianTime();

                    txn.IsVerificationRequired = false;
                    txn.IsSuccess = true;
                    txn.ErrorMessage = null;

                    context.DbAccount.Update(txn.FromAccount);
                    context.DbAccount.Update(txn.ToAccount);
                }

                context.DbTransactions.Update(txn);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return isApproved ? "Transaction approved successfully." : "Transaction rejected.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Approval failed: {ex.Message}";
            }
        }

    }
}
