using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class AccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AccountNumber { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;

        [Required]
        public DateTime CreatedAt { get; set; } = IndianTime.GetIndianTime();

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public UsersModel? User { get; set; }

        [ForeignKey(nameof(AccountType))]
        public int AccountTypeId { get; set; }
        public MasterAccountTypeModel? AccountType { get; set; }

        public DateTime? LastTransactionAt { get; set; }
        public string Currency { get; set; } = "INR";

        [Required]
        public bool IsActive { get; set; } = true;
        public DateTime? ClosedAt { get; set; }

        public bool IsAccountClosed { get; set; } = false;


    }
}
