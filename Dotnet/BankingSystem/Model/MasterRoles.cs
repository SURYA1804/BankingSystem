using System.ComponentModel.DataAnnotations;

namespace  Model;

public class MasterRoles
{
    [Key]
    public int RoleId { get; set; }
    public string? RoleName { get; set;}
}