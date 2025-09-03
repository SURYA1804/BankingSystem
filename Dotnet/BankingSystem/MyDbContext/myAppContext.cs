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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (Database.IsSqlServer())
        {
            modelBuilder.Entity<MasterCustomerType>()
                .Property(e => e.CustomerType)
                .HasColumnType("nvarchar(max)");
        }
        else if (Database.IsSqlite())
        {
            modelBuilder.Entity<MasterCustomerType>()
                .Property(e => e.CustomerType)
                .HasColumnType("TEXT");
        }

        base.OnModelCreating(modelBuilder);
    }

}

public class SqlServerDbContext : MyAppDbContext
{
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : base(options) { }
}

public class SqliteDbContext : MyAppDbContext
{
    public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
        : base(options) { }
}