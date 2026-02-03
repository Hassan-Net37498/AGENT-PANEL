using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class userrr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDeposits",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalLosses",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWagers",
                table: "Users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Country", "CreatedAt", "PhoneNumber", "TotalDeposits", "TotalLosses", "TotalWagers" },
                values: new object[] { "USA", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234567890", 1000.00m, 100.00m, 500.00m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AgentId", "Country", "CreatedAt", "Email", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "TotalDeposits", "TotalLosses", "TotalWagers" },
                values: new object[] { 2, 1, "USA", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob@example.com", "Bob Smith", false, "hashedpassword", "0987654321", 2000.00m, 200.00m, 1500.00m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalDeposits",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalLosses",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalWagers",
                table: "Users");
        }
    }
}
