namespace DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }

        public int CustomerTypeId { get; set; }
        public string? CustomerTypeName { get; set; }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public string? Address { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsEmployed { get; set; }
    }
}
