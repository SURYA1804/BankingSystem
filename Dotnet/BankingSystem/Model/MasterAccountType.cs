using System.ComponentModel.DataAnnotations;

namespace Model;

public class MasterAccountTypeModel
{
    [Key]
    public int AccountTypeID { get; set; }
    [Required]
    public required string AccountType { get; set; }
}