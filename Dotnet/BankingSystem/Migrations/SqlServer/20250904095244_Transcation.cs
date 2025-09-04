using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Transcation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbTransactionTypes",
                columns: table => new
                {
                    TransactionTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbTransactionTypes", x => x.TransactionTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DbTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    ToAccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionTypeID = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerificationRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_DbTransactions_DbTransactionTypes_TransactionTypeID",
                        column: x => x.TransactionTypeID,
                        principalTable: "DbTransactionTypes",
                        principalColumn: "TransactionTypeID",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_DbTransactions_TransactionTypeID",
                table: "DbTransactions",
                column: "TransactionTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbTransactions");

            migrationBuilder.DropTable(
                name: "DbTransactionTypes");
        }
    }
}
