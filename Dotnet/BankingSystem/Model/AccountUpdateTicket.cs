using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class AccountUpdateTicket
{
    [Key]
    public int TicketId { get; set; }

    [ForeignKey(nameof(Account))]
    public long AccountNumber { get; set; }
    public AccountModel? Account { get; set; }

    [Required]
    public string RequestedChange { get; set; } = string.Empty;

    [Required]
    public string RequestedBy { get; set; } = string.Empty;

    [Required]
    public DateTime RequestedAt { get; set; } = IndianTime.GetIndianTime();

    public bool IsApproved { get; set; } = false;
    public bool IsProcessed { get; set; } = false;

    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    
    public string? RejectionReason { get; set; }
}
