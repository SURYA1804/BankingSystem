using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class QueryComments
{
    [Key]
    public int QueryCommentsId { get; set; }

    [ForeignKey(nameof(customerQuery))]
    public int CustomerQueryId { get; set; }

    public CustomerQueryModel? customerQuery { get; set; }

    [Required]
    public required string comments { get; set; }

    public bool IsUserComment { get; set; } = true;

    public bool IsStaffComment { get; set; } = false;

    public DateTime CreatedAt { get; set; } = IndianTime.GetIndianTime();
}