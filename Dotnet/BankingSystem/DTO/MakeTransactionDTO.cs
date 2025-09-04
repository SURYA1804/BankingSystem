namespace DTO;

public class MakeTransactionDTO
{
    public long FromAccount { get; set; }
    public long ToAccount { get; set; }

    public decimal Amount { get; set; }

    public int TransactionTypeId { get; set; }
}