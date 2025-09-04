using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class LoanTypeModel
{
    [Key]
    public int LoanTypeId { get; set; }

    public required string LoanTypeName { get; set; }
}