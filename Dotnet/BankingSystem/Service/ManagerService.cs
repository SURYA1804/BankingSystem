using AutoMapper;
using DTO;
using interfaces;
using Microsoft.EntityFrameworkCore;
using Model;
using MyDbContext;

namespace Service;

public class ManagerService : IManagerService
{
    private readonly MyAppDbContext context;
    private readonly IMapper mapper;

    public ManagerService(MyAppDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<bool> CreateStaffAsync(RegisterDTO registerDTO)
    {
        try
        {
            var staffRole = await context.DbRoles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower() == "staff");

            var customerType = await context.DbCustomerTypes
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
        catch (Exception ex)
        {
            return false;
        }
    }
    public async Task<List<UserDTO>> GetAllStaffAsync()
    {
        try
        {
            var staffRole = await context.DbRoles
                .FirstOrDefaultAsync(r => r.RoleName.ToLower() == "staff");
            var staffs = await context.DbUsers.Include(r => r.Role).Include(t => t.CustomerType).Where(r => r.RoleId == staffRole!.RoleId).ToListAsync();
            return mapper.Map<List<UserDTO>>(staffs);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
            public async Task<List<UserActivityDto>> GetUserActivityAsync()
            {
                var usersWithAccounts = await context.DbUsers.Include(u => u.Account)
                .ThenInclude(a => a.AccountType).Include(m => m.Role).Include(m=>m.CustomerType)
                .Where(m=>m.Role.RoleName.ToLower() == "customer" ).ToListAsync();
                var result = mapper.Map<List<UserActivityDto>>(usersWithAccounts);
                return result;
            }

}