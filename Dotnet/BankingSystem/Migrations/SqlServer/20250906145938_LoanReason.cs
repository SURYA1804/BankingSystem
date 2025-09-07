using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class LoanReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "DbLoan",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "DbLoan");
        }
    }
}
