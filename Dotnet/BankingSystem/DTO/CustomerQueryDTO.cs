public class CustomerQueryDTO
{
    public int CustomerQueryId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public int UserId { get; set; }
    public string QueryType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool IsSolved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SolvedAt { get; set; }
    public List<QueryCommentsDTO> Comments { get; set; } = new();
}

public class QueryCommentsDTO
{
    public string CommentId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsStaffComment { get; set; }
    public bool IsUserComment { get; set; }
}
