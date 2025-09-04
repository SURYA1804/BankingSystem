using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service
{
    public class TransactionService : ITransactionService
    {
        private readonly MyAppDbContext context;

        public TransactionService(MyAppDbContext context)
        {
            this.context = context;
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





        public async Task<List<object>> GetAllTransactionsToApproveAsync()
        {
            try
            {
                var transactions = await context.DbTransactions
                    .Where(t => t.IsVerificationRequired && !t.IsSuccess)
                    .Include(t => t.FromAccount).ThenInclude(a => a.User)
                    .Include(t => t.ToAccount).ThenInclude(a => a.User)
                    .Include(t => t.TransactionType)
                    .Select(t => new
                    {
                        t.TransactionId,
                        t.TransactionDate,
                        t.IsVerificationRequired,
                        t.IsSuccess,
                        t.ErrorMessage,
                        TransactionType = t.TransactionType.TransactionType,
                        FromAccount = t.FromAccount.AccountNumber,
                        FromUser = t.FromAccount.User.Name,
                        FromEmail = t.FromAccount.User.Email,
                        ToAccount = t.ToAccount.AccountNumber,
                        ToUser = t.ToAccount.User.Name,
                        ToEmail = t.ToAccount.User.Email
                    })
                    .ToListAsync();

                return transactions.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                return new List<object>();
            }
        }

        public async Task<string> ApproveTransactionAsync(int transactionId, int staffId, bool isApproved)
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
                    txn.ErrorMessage = "Transaction rejected by staff.";
                }
                else
                {
                    if (txn.FromAccount.Balance < 0)
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
