namespace  DTO;

public class RegisterDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }

    public required string Password { get; set; }
    public required string CustomerType { get; set; }

    public int Age { get; set; }

    public DateOnly DOB { get; set; }
    public bool IsEmployed { get; set; }
    public string? Address { get; set; }
    public required string PhoneNumber { get; set; }


}