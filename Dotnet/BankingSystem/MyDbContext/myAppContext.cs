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
    
    public DbSet<QueryTypeModel> DbQueryType { get; set; }

    public DbSet<CustomerQueryModel> DbCustomerQuery { get; set; }  

    public DbSet<CustomerQueryPriorityModel> DbQueryPriority { get; set; }

    public DbSet<CustomerQueryStatusModel> DbQueryStatus { get; set; }

    public DbSet<QueryComments> DbQueryComments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<UsersModel>()
        //     .HasIndex(u => u.Email)
        //     .IsUnique();

        // modelBuilder.Entity<UsersModel>()
        //     .HasIndex(u => u.PhoneNumber)
        //     .IsUnique();

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
        modelBuilder.Entity<CustomerQueryStatusModel>().HasData(
        new CustomerQueryStatusModel { QueryStatusId = 1, StatusName = "Open" },
        new CustomerQueryStatusModel { QueryStatusId = 2, StatusName = "Pending" },
        new CustomerQueryStatusModel { QueryStatusId = 3, StatusName = "Closed" }
    );

        modelBuilder.Entity<CustomerQueryPriorityModel>().HasData(
            new CustomerQueryPriorityModel { QueryPriorityId = 1, PriorityName = "Low" },
            new CustomerQueryPriorityModel { QueryPriorityId = 2, PriorityName = "Medium" },
            new CustomerQueryPriorityModel { QueryPriorityId = 3, PriorityName = "High" },
            new CustomerQueryPriorityModel { QueryPriorityId = 4, PriorityName = "Urgent" }
        );

        modelBuilder.Entity<QueryTypeModel>().HasData(
            new QueryTypeModel { QueryTypeID = 1, QueryType = "Regrading Loan", PriorityId = 3 },
            new QueryTypeModel { QueryTypeID = 2, QueryType = "Regrading Service", PriorityId = 2 },
            new QueryTypeModel { QueryTypeID = 3, QueryType = "Regrading Bank charges", PriorityId = 1 },
            new QueryTypeModel { QueryTypeID = 4, QueryType = "Regrading Interest Rates", PriorityId = 1 },
            new QueryTypeModel { QueryTypeID = 5, QueryType = "Regrading Failed Transcation", PriorityId = 4 },
            new QueryTypeModel { QueryTypeID = 6, QueryType = "Regrading Account Creation", PriorityId = 3 }
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
                
        modelBuilder.Entity<CustomerQueryModel>()
        .HasOne(q => q.User)
        .WithMany()
        .HasForeignKey(q => q.CreatedBy)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<CustomerQueryModel>()
        .HasOne(q => q.queryType)
        .WithMany()
        .HasForeignKey(q => q.QueryTypeId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<CustomerQueryModel>()
        .HasOne(q => q.QueryStatus)
        .WithMany()
        .HasForeignKey(q => q.StatusId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<CustomerQueryModel>()
        .HasOne(q => q.QueryPriority)
        .WithMany()
        .HasForeignKey(q => q.PriorityId)
        .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<QueryComments>()
        .HasOne(c => c.customerQuery)
        .WithMany(q => q.QueryComments)
        .HasForeignKey(c => c.CustomerQueryId)
        .OnDelete(DeleteBehavior.Cascade);


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