using System.Net;
using System.Net.Mail;
using DTO;
using interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Security.Cryptography;
using MyDbContext;
using System.Text;
using Microsoft.Extensions.Options;

namespace Service;

public class UserService : IUserService
{
    private readonly MyAppDbContext context;
    private readonly EmailCredentialsDTO emailCredentials;
    public UserService(MyAppDbContext context, IOptions<EmailCredentialsDTO> emailOptions)
    {
        this.context = context;
        this.emailCredentials = emailOptions.Value;
         if (string.IsNullOrEmpty(emailCredentials.Email) || string.IsNullOrEmpty(emailCredentials.Password))
        {
            throw new ArgumentException("EmailCredentials not configured properly in appsettings.json");
        }
    }

    // public async Task<> LoginAsync(LoginDTO loginDTO)
    // {
    //     return  
    // }

   public async Task<bool> RegisterCustomerAsync(RegisterDTO registerDTO)
{
    using var transaction = await context.Database.BeginTransactionAsync();
    try
    {
        if (await context.DbUsers.AnyAsync(u => u.Email == registerDTO.Email))
            throw new Exception("User with this email already exists");

        var hashedPassword = HashPassword(registerDTO.Password);
        var customerType = await context.DbCustomerTypes
            .FirstAsync(c => c.CustomerType.ToLower() == registerDTO.CustomerType.ToLower());

        var user = new UsersModel
        {
            Name = registerDTO.Name!,
            Email = registerDTO.Email!,
            Password = hashedPassword,
            Age = registerDTO.Age,
            DOB = registerDTO.DOB,
            IsEmployed = registerDTO.IsEmployed,
            Address = registerDTO.Address,
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            PhoneNumber = registerDTO.PhoneNumber!,
            RoleId = 2,
            IsVerified = false,
            CustomerTypeId = customerType.CustomerTypeId
        };

        context.DbUsers.Add(user);
        await context.SaveChangesAsync();

        var otp = new Random().Next(100000, 999999);

        var otpRecord = await context.DbOTP.FirstOrDefaultAsync(o => o.Email == registerDTO.Email);
        if (otpRecord != null)
        {
            otpRecord.OTP = otp;
            otpRecord.ExpiryTime = DateTime.Now.AddMinutes(5);
            context.DbOTP.Update(otpRecord);
        }
        else
        {
            var otpEntry = new OTPValidationModel
            {
                Email = registerDTO.Email,
                OTP = otp,
                ExpiryTime = DateTime.Now.AddMinutes(5)
            };
            context.DbOTP.Add(otpEntry);
        }
        await context.SaveChangesAsync();

        var smtp = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailCredentials.Email, emailCredentials.Password),
            EnableSsl = true,
        };

        var mail = new MailMessage(emailCredentials.Email, registerDTO.Email!)
        {
            Subject = "Your Bank OTP Verification",
            Body = $"Your OTP is: {otp}. It will expire in 5 minutes."
        };

        await smtp.SendMailAsync(mail);

        await transaction.CommitAsync();
        return true;
    }
    catch(Exception ex)
    {
        await transaction.RollbackAsync();
        return false;
    }
}

    public async Task<bool> VerifyOtpAsync(string email, int otp)
    {
        try
        {
            var otpRecord = await context.DbOTP.FirstOrDefaultAsync(o => o.Email == email && o.OTP == otp);

            if (otpRecord == null)
                return false;
            if (otpRecord.ExpiryTime < DateTime.Now)
                return false;

            var user = await context.DbUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.IsVerified = true;
            context.DbOTP.Remove(otpRecord);
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private string HashPassword(string password)
    {
        try
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        catch (Exception ex)
        {
            return "";
        }
    }

}