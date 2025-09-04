namespace DTO;

public class AddCommentsDTO
{
    public int QueryId { get; set; }
    public required string Comments { get; set; }

    public bool isStaff { get; set; } = false;

}