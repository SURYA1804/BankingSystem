namespace DTO;

public class AccountDTO
{
    public long AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }             
    public string UserName { get; set; } = string.Empty;

    public string AccountTypeName { get; set; } = string.Empty;
    public int AccountTypeId { get; set; }      
    public DateTime? LastTransactionAt { get; set; }
    public string Currency { get; set; } = "INR";
    public bool IsActive { get; set; }
    public bool IsAccountClosed { get; set; }
}