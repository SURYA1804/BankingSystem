using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

public class UsersModel
{
    [Key]
    public int UserId { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(1, 120)]
    public int Age { get; set; }

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly DOB { get; set; }

    [ForeignKey(nameof(CustomerType))]
    public int CustomerTypeId { get; set; }

    public MasterCustomerType? CustomerType { get; set; }
    public bool IsEmployed { get; set; }

    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }

    public MasterRoles? Role { get; set; }

    public string? Address { get; set; }

    public DateOnly CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    
    public bool IsVerified { get; set; }
}
