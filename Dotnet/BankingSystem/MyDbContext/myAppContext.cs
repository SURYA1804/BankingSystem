namespace MyDbContext;

using Microsoft.EntityFrameworkCore;
using Model;

public class MyAppDbContext : DbContext
{
    public MyAppDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<UsersModel> DbUsers { get; set; }

    public DbSet<MasterRoles> DbRoles { get; set; }

    public DbSet<MasterCustomerType> DbCustomerTypes { get; set; }  

    public DbSet<OTPValidationModel> DbOTP { get; set; }

}