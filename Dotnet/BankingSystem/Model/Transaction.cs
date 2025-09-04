using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class TransactionModel
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey(nameof(FromAccountNumber))]

        public long FromAccountNumber { get; set; }

        [ForeignKey(nameof(FromAccountNumber))]
        public AccountModel FromAccount { get; set; } = null!;

        [Required]
        public long ToAccountNumber { get; set; }

        [ForeignKey(nameof(ToAccountNumber))]
        public AccountModel ToAccount { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [ForeignKey("TransactionType")]
        public int TransactionTypeID { get; set; }
        public MasterTransactionType? TransactionType { get; set; }

        public DateTime TransactionDate { get; set; } = IndianTime.GetIndianTime();



        public bool IsVerificationRequired { get; set; } = false;

        public bool IsSuccess { get; set; }

        public string? ErrorMessage { get; set; }

        public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 12);




    }
}
