using System.ComponentModel.DataAnnotations;

namespace Model;

public class MasterCustomerType
{
    [Key]
    public int CustomerTypeId { get; set; }
    public string? CustomerType{ get; set;}
}