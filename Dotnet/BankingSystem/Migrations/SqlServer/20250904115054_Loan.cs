using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Loan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbLoanType",
                columns: table => new
                {
                    LoanTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbLoanType", x => x.LoanTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DbLoan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    CurrentSalaryInLPA = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DbTransactions_FromAccountNumber",
                table: "DbTransactions",
                column: "FromAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DbTransactions_ToAccountNumber",
                table: "DbTransactions",
                column: "ToAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DbLoan_LoanTypeId",
                table: "DbLoan",
                column: "LoanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbLoan_UserId",
                table: "DbLoan",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbTransactions_DbAccount_FromAccountNumber",
                table: "DbTransactions",
                column: "FromAccountNumber",
                principalTable: "DbAccount",
                principalColumn: "AccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_DbTransactions_DbAccount_ToAccountNumber",
                table: "DbTransactions",
                column: "ToAccountNumber",
                principalTable: "DbAccount",
                principalColumn: "AccountNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbTransactions_DbAccount_FromAccountNumber",
                table: "DbTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_DbTransactions_DbAccount_ToAccountNumber",
                table: "DbTransactions");

            migrationBuilder.DropTable(
                name: "DbLoan");

            migrationBuilder.DropTable(
                name: "DbLoanType");

            migrationBuilder.DropIndex(
                name: "IX_DbTransactions_FromAccountNumber",
                table: "DbTransactions");

            migrationBuilder.DropIndex(
                name: "IX_DbTransactions_ToAccountNumber",
                table: "DbTransactions");
        }
    }
}
