using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingSystem.Migrations.Postgress
{
    /// <inheritdoc />
    public partial class Latest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbAccountType",
                columns: table => new
                {
                    AccountTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbAccountType", x => x.AccountTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DbCustomerTypes",
                columns: table => new
                {
                    CustomerTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbCustomerTypes", x => x.CustomerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DbLoanType",
                columns: table => new
                {
                    LoanTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoanTypeName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLoanType", x => x.LoanTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DbOTP",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    OTP = table.Column<int>(type: "integer", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbOTP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DbQueryPriority",
                columns: table => new
                {
                    QueryPriorityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PriorityName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbQueryPriority", x => x.QueryPriorityId);
                });

            migrationBuilder.CreateTable(
                name: "DbQueryStatus",
                columns: table => new
                {
                    QueryStatusId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StatusName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbQueryStatus", x => x.QueryStatusId);
                });

            migrationBuilder.CreateTable(
                name: "DbRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbRoles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "DbTransactionTypes",
                columns: table => new
                {
                    TransactionTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbTransactionTypes", x => x.TransactionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DbQueryType",
                columns: table => new
                {
                    QueryTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QueryType = table.Column<string>(type: "text", nullable: false),
                    PriorityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbQueryType", x => x.QueryTypeID);
                    table.ForeignKey(
                        name: "FK_DbQueryType_DbQueryPriority_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "DbQueryPriority",
                        principalColumn: "QueryPriorityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    CustomerTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsEmployed = table.Column<bool>(type: "boolean", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_DbUsers_DbCustomerTypes_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "DbCustomerTypes",
                        principalColumn: "CustomerTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbUsers_DbRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "DbRoles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbAccount",
                columns: table => new
                {
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    LastTransactionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsAccountClosed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbAccount", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_DbAccount_DbAccountType_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "DbAccountType",
                        principalColumn: "AccountTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbAccount_DbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DbUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbCustomerQuery",
                columns: table => new
                {
                    CustomerQueryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QueryTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SolvedBy = table.Column<int>(type: "integer", nullable: true),
                    SolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsSolved = table.Column<bool>(type: "boolean", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    PriorityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbCustomerQuery", x => x.CustomerQueryId);
                    table.ForeignKey(
                        name: "FK_DbCustomerQuery_DbQueryPriority_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "DbQueryPriority",
                        principalColumn: "QueryPriorityId");
                    table.ForeignKey(
                        name: "FK_DbCustomerQuery_DbQueryStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DbQueryStatus",
                        principalColumn: "QueryStatusId");
                    table.ForeignKey(
                        name: "FK_DbCustomerQuery_DbQueryType_QueryTypeId",
                        column: x => x.QueryTypeId,
                        principalTable: "DbQueryType",
                        principalColumn: "QueryTypeID");
                    table.ForeignKey(
                        name: "FK_DbCustomerQuery_DbUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "DbUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DbLoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoanTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApprovedBy = table.Column<int>(type: "integer", nullable: false),
                    LoanAmount = table.Column<int>(type: "integer", nullable: false),
                    CurrentSalaryInLPA = table.Column<int>(type: "integer", nullable: false),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    isProcessed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLoan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbLoan_DbLoanType_LoanTypeId",
                        column: x => x.LoanTypeId,
                        principalTable: "DbLoanType",
                        principalColumn: "LoanTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbLoan_DbUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DbUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbAccountUpdateTickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    RequestedChange = table.Column<string>(type: "text", nullable: false),
                    RequestedBy = table.Column<string>(type: "text", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedBy = table.Column<string>(type: "text", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbAccountUpdateTickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_DbAccountUpdateTickets_DbAccount_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "DbAccount",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromAccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    ToAccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionTypeID = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsVerificationRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_DbTransactions_DbAccount_FromAccountNumber",
                        column: x => x.FromAccountNumber,
                        principalTable: "DbAccount",
                        principalColumn: "AccountNumber");
                    table.ForeignKey(
                        name: "FK_DbTransactions_DbAccount_ToAccountNumber",
                        column: x => x.ToAccountNumber,
                        principalTable: "DbAccount",
                        principalColumn: "AccountNumber");
                    table.ForeignKey(
                        name: "FK_DbTransactions_DbTransactionTypes_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "DbTransactionTypes",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbQueryComments",
                columns: table => new
                {
                    QueryCommentsId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerQueryId = table.Column<int>(type: "integer", nullable: false),
                    comments = table.Column<string>(type: "text", nullable: false),
                    IsUserComment = table.Column<bool>(type: "boolean", nullable: false),
                    IsStaffComment = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbQueryComments", x => x.QueryCommentsId);
                    table.ForeignKey(
                        name: "FK_DbQueryComments_DbCustomerQuery_CustomerQueryId",
                        column: x => x.CustomerQueryId,
                        principalTable: "DbCustomerQuery",
                        principalColumn: "CustomerQueryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DbAccountType",
                columns: new[] { "AccountTypeID", "AccountType" },
                values: new object[,]
                {
                    { 1, "Savings" },
                    { 2, "Current" },
                    { 3, "Salary" }
                });

            migrationBuilder.InsertData(
                table: "DbCustomerTypes",
                columns: new[] { "CustomerTypeId", "CustomerType" },
                values: new object[,]
                {
                    { 1, "Individual" },
                    { 2, "Company" },
                    { 3, "Minnor" },
                    { 4, "Joint" },
                    { 5, "Nil" }
                });

            migrationBuilder.InsertData(
                table: "DbLoanType",
                columns: new[] { "LoanTypeId", "LoanTypeName" },
                values: new object[,]
                {
                    { 1, "Personal Loan" },
                    { 2, "Home Loan" },
                    { 3, "Car Loan" },
                    { 4, "Education Loan" }
                });

            migrationBuilder.InsertData(
                table: "DbQueryPriority",
                columns: new[] { "QueryPriorityId", "PriorityName" },
                values: new object[,]
                {
                    { 1, "Low" },
                    { 2, "Medium" },
                    { 3, "High" },
                    { 4, "Urgent" }
                });

            migrationBuilder.InsertData(
                table: "DbQueryStatus",
                columns: new[] { "QueryStatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "Pending" },
                    { 3, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "DbRoles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Customer" },
                    { 2, "Staff" },
                    { 3, "Manager" }
                });

            migrationBuilder.InsertData(
                table: "DbTransactionTypes",
                columns: new[] { "TransactionTypeID", "TransactionType" },
                values: new object[,]
                {
                    { 1, "Deposit" },
                    { 2, "Withdrawal" },
                    { 3, "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "DbQueryType",
                columns: new[] { "QueryTypeID", "PriorityId", "QueryType" },
                values: new object[,]
                {
                    { 1, 3, "Regrading Loan" },
                    { 2, 2, "Regrading Service" },
                    { 3, 1, "Regrading Bank charges" },
                    { 4, 1, "Regrading Interest Rates" },
                    { 5, 4, "Regrading Failed Transcation" },
                    { 6, 3, "Regrading Account Creation" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbAccount_AccountTypeId",
                table: "DbAccount",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbAccount_UserId",
                table: "DbAccount",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DbAccountUpdateTickets_AccountNumber",
                table: "DbAccountUpdateTickets",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DbCustomerQuery_CreatedBy",
                table: "DbCustomerQuery",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DbCustomerQuery_PriorityId",
                table: "DbCustomerQuery",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_DbCustomerQuery_QueryTypeId",
                table: "DbCustomerQuery",
                column: "QueryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbCustomerQuery_StatusId",
                table: "DbCustomerQuery",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DbLoan_LoanTypeId",
                table: "DbLoan",
                column: "LoanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbLoan_UserId",
                table: "DbLoan",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DbQueryComments_CustomerQueryId",
                table: "DbQueryComments",
                column: "CustomerQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_DbQueryType_PriorityId",
                table: "DbQueryType",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_DbTransactions_FromAccountNumber",
                table: "DbTransactions",
                column: "FromAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DbTransactions_ToAccountNumber",
                table: "DbTransactions",
                column: "ToAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DbTransactions_TransactionTypeID",
                table: "DbTransactions",
                column: "TransactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_DbUsers_CustomerTypeId",
                table: "DbUsers",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbUsers_RoleId",
                table: "DbUsers",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbAccountUpdateTickets");

            migrationBuilder.DropTable(
                name: "DbLoan");

            migrationBuilder.DropTable(
                name: "DbOTP");

            migrationBuilder.DropTable(
                name: "DbQueryComments");

            migrationBuilder.DropTable(
                name: "DbTransactions");

            migrationBuilder.DropTable(
                name: "DbLoanType");

            migrationBuilder.DropTable(
                name: "DbCustomerQuery");

            migrationBuilder.DropTable(
                name: "DbAccount");

            migrationBuilder.DropTable(
                name: "DbTransactionTypes");

            migrationBuilder.DropTable(
                name: "DbQueryStatus");

            migrationBuilder.DropTable(
                name: "DbQueryType");

            migrationBuilder.DropTable(
                name: "DbAccountType");

            migrationBuilder.DropTable(
                name: "DbUsers");

            migrationBuilder.DropTable(
                name: "DbQueryPriority");

            migrationBuilder.DropTable(
                name: "DbCustomerTypes");

            migrationBuilder.DropTable(
                name: "DbRoles");
        }
    }
}
