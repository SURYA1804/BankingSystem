namespace Model.DTOs;

public class AccountUpdateTicketDTO
{
    public int TicketId { get; set; }
    public long AccountNumber { get; set; }
    public string RequestedChange { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }
    public bool IsApproved { get; set; }
    public bool IsProcessed { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReaosn { get; set; }
}
