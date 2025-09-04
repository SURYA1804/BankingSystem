using System.ComponentModel.DataAnnotations;

namespace Model;

public class CustomerQueryStatusModel
{
    [Key]
    public int QueryStatusId { get; set; }

    [Required]
    public required string StatusName { get; set; }  
}