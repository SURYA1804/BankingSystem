using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "AccountNumberSeq",
                startValue: 1000000000L);

            migrationBuilder.CreateTable(
                name: "DbAccountType",
                columns: table => new
                {
                    AccountTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbAccountType", x => x.AccountTypeID);
                });

            migrationBuilder.CreateTable(
                name: "DbAccount",
                columns: table => new
                {
                    AccountNumber = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "NEXT VALUE FOR AccountNumberSeq"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    LastTransactionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAccountClosed = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DbAccount_AccountTypeId",
                table: "DbAccount",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DbAccount_UserId",
                table: "DbAccount",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbAccount");

            migrationBuilder.DropTable(
                name: "DbAccountType");

            migrationBuilder.DropSequence(
                name: "AccountNumberSeq");
        }
    }
}
