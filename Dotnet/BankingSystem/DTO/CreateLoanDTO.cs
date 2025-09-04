namespace DTO;

public class CreateLoanDTO
{
    public int UserId { get; set; }
    public int LoanTypeId { get; set; }
    public int LoanAmount { get; set; }

    public int CurrentSalaryInLPA { get; set; }

}