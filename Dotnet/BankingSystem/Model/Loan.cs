using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class LoanModel
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(LoanType))]
    public int LoanTypeId { get; set; }
    public LoanTypeModel? LoanType { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = IndianTime.GetIndianTime();

    public bool IsApproved { get; set; } = false;

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public UsersModel? User { get; set; } = null!;

    public DateTime? ApprovedAt { get; set; }

    public int ApprovedBy { get; set; }
    
    public int CurrentSalaryInLPA { get; set; }
    
}
