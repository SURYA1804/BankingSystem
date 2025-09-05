namespace Model.DTOs;

public class LoanDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int LoanTypeId { get; set; }
    public string? LoanTypeName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsApproved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public int ApprovedBy { get; set; }
    public int CurrentSalaryInLPA { get; set; }
    public string? CustomerType { get; set; }

    public bool IsEmployed { get; set; }

}
