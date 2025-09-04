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
    
    public DbSet<AccountModel> DbAccount { get; set; }

    public DbSet<MasterAccountTypeModel> DbAccountType { get; set; }

    public DbSet<AccountUpdateTicket> DbAccountUpdateTickets { get; set; }
    public DbSet<TransactionModel> DbTransactions { get; set; }
    public DbSet<MasterTransactionType> DbTransactionTypes { get; set; }

    public DbSet<LoanTypeModel> DbLoanType { get; set; }
    public DbSet<LoanModel> DbLoan { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UsersModel>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UsersModel>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<UsersModel>()
            .HasIndex(u => u.UserName)
            .IsUnique();
        modelBuilder.HasSequence<long>("AccountNumberSeq")
    .StartsAt(1000000000)
    .IncrementsBy(1);

        modelBuilder.Entity<AccountModel>()
            .Property(a => a.AccountNumber)
            .HasDefaultValueSql("NEXT VALUE FOR AccountNumberSeq");

        modelBuilder.Entity<MasterTransactionType>().HasData(
               new MasterTransactionType { TransactionTypeID = 1, TransactionType = "Deposit" },
               new MasterTransactionType { TransactionTypeID = 2, TransactionType = "Withdrawal" },
               new MasterTransactionType { TransactionTypeID = 3, TransactionType = "Transfer" }
           );
        modelBuilder.Entity<LoanTypeModel>().HasData(
            new LoanTypeModel { LoanTypeId = 1, LoanTypeName = "Personal Loan" },
            new LoanTypeModel { LoanTypeId = 2, LoanTypeName = "Home Loan" },
            new LoanTypeModel { LoanTypeId = 3, LoanTypeName = "Car Loan" },
            new LoanTypeModel { LoanTypeId = 4, LoanTypeName = "Education Loan" }
        );
            modelBuilder.Entity<TransactionModel>()
        .HasOne(t => t.FromAccount)
        .WithMany()
        .HasForeignKey(t => t.FromAccountNumber)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<TransactionModel>()
        .HasOne(t => t.ToAccount)
        .WithMany()
        .HasForeignKey(t => t.ToAccountNumber)
        .OnDelete(DeleteBehavior.NoAction);

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