using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingSystem.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class Latest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
            migrationBuilder.Sql("UPDATE sqlite_sequence SET seq = 99999999 WHERE name = 'DbAccount';");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DbAccountType",
                keyColumn: "AccountTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DbAccountType",
                keyColumn: "AccountTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DbAccountType",
                keyColumn: "AccountTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DbCustomerTypes",
                keyColumn: "CustomerTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DbCustomerTypes",
                keyColumn: "CustomerTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DbCustomerTypes",
                keyColumn: "CustomerTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DbCustomerTypes",
                keyColumn: "CustomerTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DbCustomerTypes",
                keyColumn: "CustomerTypeId",
                keyValue: 5);
        }
    }
}
