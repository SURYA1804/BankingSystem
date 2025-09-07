namespace Model.DTOs;

public class TransactionDTO
{
    public int TransactionId { get; set; }

    public string? ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public bool IsVerificationRequired { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public long FromAccount { get; set; }
    public string FromUser { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;

    public long ToAccount { get; set; }
    public string ToUser { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
