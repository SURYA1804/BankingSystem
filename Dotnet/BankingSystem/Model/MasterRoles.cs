using System.ComponentModel.DataAnnotations;

namespace  Model;

public class MasterRoles
{
    [Key]
    public int RoleId { get; set; }
    public required string RoleName { get; set;}
}