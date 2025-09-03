using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyDbContext;

public class DbContextFactory : IDesignTimeDbContextFactory<SqliteDbContext>
{
    public SqliteDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var cs = cfg.GetConnectionString("SqliteConnection") ?? "Data Source=BankingSystem.db";
        var opts = new DbContextOptionsBuilder<SqliteDbContext>().UseSqlite(cs).Options;
        return new SqliteDbContext(opts);
    }
}

public class SqlServerDbContextFactory : IDesignTimeDbContextFactory<SqlServerDbContext>
{
    public SqlServerDbContext CreateDbContext(string[] args)
    {
        var cfg = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var cs = cfg.GetConnectionString("DefaultConnection")
                 ?? "Server=PTPLL487;Database=BankingSystem;Integrated Security=true;TrustServerCertificate=True;";
        var opts = new DbContextOptionsBuilder<SqlServerDbContext>().UseSqlServer(cs).Options;
        return new SqlServerDbContext(opts);
    }
}
