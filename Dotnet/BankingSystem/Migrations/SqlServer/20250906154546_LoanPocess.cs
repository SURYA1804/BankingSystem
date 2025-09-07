using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class LoanPocess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isProcessed",
                table: "DbLoan",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isProcessed",
                table: "DbLoan");
        }
    }
}
