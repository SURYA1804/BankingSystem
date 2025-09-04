using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingSystem.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class CustomerSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbQueryPriority",
                columns: table => new
                {
                    QueryPriorityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbQueryPriority", x => x.QueryPriorityId);
                });

            migrationBuilder.CreateTable(
                name: "DbQueryStatus",
                columns: table => new
                {
                    QueryStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbQueryStatus", x => x.QueryStatusId);
                });

            migrationBuilder.CreateTable(
                name: "DbQueryType",
                columns: table => new
                {
                    QueryTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriorityId = table.Column<int>(type: "int", nullable: false)
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
                name: "DbCustomerQuery",
                columns: table => new
                {
                    CustomerQueryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueryTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolvedBy = table.Column<int>(type: "int", nullable: true),
                    SolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSolved = table.Column<bool>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    PriorityId = table.Column<int>(type: "int", nullable: false)
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
                name: "DbQueryComments",
                columns: table => new
                {
                    QueryCommentsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerQueryId = table.Column<int>(type: "int", nullable: false),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUserComment = table.Column<bool>(type: "bit", nullable: false),
                    IsStaffComment = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_DbQueryComments_CustomerQueryId",
                table: "DbQueryComments",
                column: "CustomerQueryId");

            migrationBuilder.CreateIndex(
                name: "IX_DbQueryType_PriorityId",
                table: "DbQueryType",
                column: "PriorityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbQueryComments");

            migrationBuilder.DropTable(
                name: "DbCustomerQuery");

            migrationBuilder.DropTable(
                name: "DbQueryStatus");

            migrationBuilder.DropTable(
                name: "DbQueryType");

            migrationBuilder.DropTable(
                name: "DbQueryPriority");
        }
    }
}
