using System.ComponentModel.DataAnnotations;

namespace Model;

public class CustomerQueryPriorityModel
{
    [Key]
    public int QueryPriorityId { get; set; }

    [Required]
    public required string PriorityName { get; set; }  
}
