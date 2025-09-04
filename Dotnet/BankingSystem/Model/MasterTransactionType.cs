using System.ComponentModel.DataAnnotations;


namespace Model;

public class MasterTransactionType
{
    [Key]
    public int TransactionTypeID { get; set; }
    public required string TransactionType { get; set; }
}
