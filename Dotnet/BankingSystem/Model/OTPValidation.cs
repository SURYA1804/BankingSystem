using System.ComponentModel.DataAnnotations;

namespace Model;

public class OTPValidationModel
{
    [Key]
    public int ID { get; set; }
    public string? Email { get; set; }

    public int OTP { get; set; }

    public DateTime ExpiryTime { get; set; }

}