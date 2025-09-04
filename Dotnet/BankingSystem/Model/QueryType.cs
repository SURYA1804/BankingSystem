using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class QueryTypeModel
{
    [Key]
    public int QueryTypeID { get; set; }

    [Required]
    public required string QueryType { get; set; }


    [ForeignKey(nameof(queryPriority))]
    public int PriorityId { get; set; }
    
    public CustomerQueryPriorityModel? queryPriority { get; set; }

}