using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class ManagerService : IManagerService
{
    private readonly MyAppDbContext context;
    public ManagerService(MyAppDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CreateStaffAsync(RegisterDTO registerDTO)
    {
        try
        {
            var staffRole = await context.DbRoles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower() == "staff");

            var customerType = await  context.DbCustomerTypes
                .FirstOrDefaultAsync(r => r.CustomerType.ToLower() == "nil");
            if (staffRole == null)
                return false;
            var hashedPassword = HassPassword.GetHashPassword(registerDTO.Password);

            var user = new UsersModel
            {
                Name = registerDTO.Name,
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                Password = hashedPassword,
                Age = registerDTO.Age,
                DOB = registerDTO.DOB,
                IsEmployed = registerDTO.IsEmployed,
                Address = registerDTO.Address,
                PhoneNumber = registerDTO.PhoneNumber,
                RoleId = staffRole.RoleId,
                CustomerTypeId = customerType.CustomerTypeId, 
                CreatedAt = DateOnly.FromDateTime(IndianTime.GetIndianTime()),
                IsVerified = true
            };

            await context.DbUsers.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}