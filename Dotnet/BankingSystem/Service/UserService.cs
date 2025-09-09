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
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Service;

public class UserService : IUserService
{
    private readonly MyAppDbContext context;
    private readonly IMapper mapper;
    private readonly EmailCredentialsDTO emailCredentials;
    private SmtpClient smpt;
    public UserService(MyAppDbContext context, IOptions<EmailCredentialsDTO> emailOptions,IMapper mapper)
    {
        this.context = context;
        this.emailCredentials = emailOptions.Value;
        this.mapper = mapper;
        if (string.IsNullOrEmpty(emailCredentials.Email) || string.IsNullOrEmpty(emailCredentials.Password))
        {
            throw new ArgumentException("EmailCredentials not configured properly in appsettings.json");
        }
        this.smpt = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailCredentials.Email, emailCredentials.Password),
            EnableSsl = true,
        };
    }
    public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
    {
        try
        {
            var hashedPassword = HassPassword.GetHashPassword(loginDTO.Password);
            var user = await context.DbUsers.Include(r=>r.Role).Include(t=>t.CustomerType)
            .FirstAsync(m => m.UserName.ToLower() == loginDTO.UserName.ToLower() && m.Password == hashedPassword );
            if (user == null)
            {
                return null;
            }
            user.LastLoginAt = IndianTime.GetIndianTime();
            await context.SaveChangesAsync();
            await GetOTPAsync(user.Email);
            return mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            return null;
        }

        
    }

    public async Task<bool> RegisterCustomerAsync(RegisterDTO registerDTO)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // if (await context.DbUsers.AnyAsync(u => u.Email == registerDTO.Email))
            //     throw new Exception("User with this email already exists");

            var hashedPassword = HassPassword.GetHashPassword(registerDTO.Password);
            // var customerType = await context.DbCustomerTypes
            //     .FirstAsync(c => c.CustomerType.ToLower() == registerDTO.CustomerType.ToLower());
            var Role = await context.DbRoles.FirstAsync(r => r.RoleName.ToLower() == "customer");
            var user = new UsersModel
            {
                Name = registerDTO.Name!,
                Email = registerDTO.Email!,
                Password = hashedPassword,
                Age = registerDTO.Age,
                DOB = registerDTO.DOB,
                IsEmployed = registerDTO.IsEmployed,
                Address = registerDTO.Address,
                CreatedAt = DateOnly.FromDateTime(IndianTime.GetIndianTime()),
                PhoneNumber = registerDTO.PhoneNumber!,
                RoleId = Role.RoleId,
                IsVerified = false,
                CustomerTypeId = Convert.ToInt32(registerDTO.CustomerType),
                UserName = registerDTO.UserName
            };

            context.DbUsers.Add(user);
            await context.SaveChangesAsync();

            var otp = new Random().Next(100000, 999999);

            var otpRecord = await context.DbOTP.FirstOrDefaultAsync(o => o.Email == registerDTO.Email);
            if (otpRecord != null)
            {
                otpRecord.OTP = otp;
                otpRecord.ExpiryTime = IndianTime.GetIndianTime().AddMinutes(5);
                context.DbOTP.Update(otpRecord);
            }
            else
            {
                var otpEntry = new OTPValidationModel
                {
                    Email = registerDTO.Email,
                    OTP = otp,
                    ExpiryTime = IndianTime.GetIndianTime().AddMinutes(5)
                };
                context.DbOTP.Add(otpEntry);
            }
            await context.SaveChangesAsync();

            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "OTP.html");
            string template = File.ReadAllText(templatePath);
            string body = template
                .Replace("{NAME}", registerDTO.Name)
                .Replace("{OTP}", otp.ToString());
            var mail = new MailMessage(emailCredentials.Email, registerDTO.Email!)
            {
                Subject = "Your Bank OTP Verification",
                Body = body,
                IsBodyHtml = true
            };


            await smpt.SendMailAsync(mail);

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> GetOTPAsync(string email)
    {
        try
        {
            var otp = new Random().Next(100000, 999999);
            var otpRecord = await context.DbOTP.FirstOrDefaultAsync(o => o.Email == email);
            if (otpRecord != null)
            {
                otpRecord.OTP = otp;
                otpRecord.ExpiryTime = IndianTime.GetIndianTime().AddMinutes(5);
                context.DbOTP.Update(otpRecord);
            }
            else
            {
                var otpEntry = new OTPValidationModel
                {
                    Email = email,
                    OTP = otp,
                    ExpiryTime = IndianTime.GetIndianTime().AddMinutes(5)
                };
                context.DbOTP.Add(otpEntry);
            }
            await context.SaveChangesAsync();
            var user = await context.DbUsers.FirstAsync(u => u.Email.ToLower() == email.ToLower());
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "OTP.html");
            string template = File.ReadAllText(templatePath);
            string body = template
                .Replace("{NAME}", user.Name)
                .Replace("{OTP}", otp.ToString());
            var mail = new MailMessage(emailCredentials.Email, email!)
            {
                Subject = "Your Bank OTP Verification",
                Body = body,
                IsBodyHtml = true
            };
            await smpt.SendMailAsync(mail);

            return true;
            
        }
        catch (Exception ex)
        {
            return false;
        }


    }
    public async Task<UsersModel?> GetUserByIdAsync(int userId)
    {
        return await context.DbUsers.FindAsync(userId);
    }
    public async Task<bool> UpdateUserAsync(int userId, JsonPatchDocument<UsersModel> patchDoc)
    {
        var user = await GetUserByIdAsync(userId);
        if (user == null) return false;

        patchDoc.ApplyTo(user);
        
        if (patchDoc.Operations.Any(op =>
        (op.path.Equals("/Password", StringComparison.OrdinalIgnoreCase)
             || op.path.Equals("Password", StringComparison.OrdinalIgnoreCase))
                && !string.IsNullOrWhiteSpace(user.Password)))
        {
            user.Password = HassPassword.GetHashPassword(user.Password);
        }

        await context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> CheckPasswordAsync(int userId, string password)
    {
        try
        {
            var user = await context.DbUsers.FirstAsync(m=>m.UserId == userId);
            var hashpassword = HassPassword.GetHashPassword(password);
            if (user.Password == hashpassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public async Task<string> VerifyOtpAsync(string email, int otp)
    {
        try
        {
            var otpRecord = await context.DbOTP.FirstOrDefaultAsync(o => o.Email == email && o.OTP == otp);

            if (otpRecord == null)
                return "OTP Not Found";
            if (otpRecord.ExpiryTime < IndianTime.GetIndianTime())
                return "OTP Expired";

            var user = await context.DbUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return "User Not Found";
            if (!user.IsVerified)
            {
                SendWelcomeMailAsync(user);
                user.IsVerified = true;
            }
            context.DbOTP.Remove(otpRecord);
            await context.SaveChangesAsync();

            return "success";
        }
        catch (Exception ex)
        {
            return "failed";
        }
    }
   
    private async void SendWelcomeMailAsync(UsersModel user)
    {
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "welcome.html");
        string template = File.ReadAllText(templatePath);
        string body = template.Replace("{Name}", user.Name).Replace("{UserName}",user.UserName).
        Replace("{LoginUrl}","alterego.bank");
        var mail = new MailMessage(emailCredentials.Email, user.Email)
        {
            Subject = "Welcome To Alter Ego Bank",
            Body = body,
            IsBodyHtml = true
        };
        await smpt.SendMailAsync(mail);
    }

}