public class AccountActivityDto
{
    public DateTime? LastTransactionAt { get; set; }
    public long AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public string AccountTypeName { get; set; } = string.Empty;
}

public class UserActivityDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; }

    public string CustomerType { get; set; }
    public DateTime LastLoginAt { get; set; }
    public bool IsVerified { get; set; }
    public List<AccountActivityDto> AccountsList { get; set; } = new List<AccountActivityDto>();
}
