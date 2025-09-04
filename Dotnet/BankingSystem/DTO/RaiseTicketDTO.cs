namespace DTO;

public class RaiseTicketDTO
{
    public int UserId { get; set; }

    public int TicketTypeId { get; set; }

    public required string Message { get; set; }
}