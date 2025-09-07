using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class LoanAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanAmount",
                table: "DbLoan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanAmount",
                table: "DbLoan");
        }
    }
}
