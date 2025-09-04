namespace DTO;

public class AccountCreationDTO
{
    public int UserId { get; set; }

    public required string AccountType { get; set; }

    public int OpeningBalance { get; set; }
    

}