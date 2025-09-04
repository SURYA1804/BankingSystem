using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class CustomerQueryModel
{
    [Key]
    public int CustomerQueryId { get; set; }
    public int QueryTypeId { get; set; }
    public QueryTypeModel? queryType { get; set; }

    [ForeignKey(nameof(User))]
    public int CreatedBy { get; set; }
    public UsersModel? User { get; set; }

    public DateTime CreatedAt { get; set; } = IndianTime.GetIndianTime();

    public int? SolvedBy { get; set; }
    public DateTime? SolvedAt { get; set; }

    public bool IsSolved { get; set; } = false;

    [ForeignKey(nameof(QueryStatus))]
    public int StatusId { get; set; }
    public CustomerQueryStatusModel? QueryStatus { get; set; }

    [ForeignKey(nameof(QueryPriority))]
    public int PriorityId { get; set; }
    public CustomerQueryPriorityModel? QueryPriority { get; set; }
    public ICollection<QueryComments>? QueryComments { get; set; } = new List<QueryComments>();

}