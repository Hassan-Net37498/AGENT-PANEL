using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class comissionn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "Commissions",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                table: "Commissions",
                type: "decimal(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "EarnedDate",
                table: "Commissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Commissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Commissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Commissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Commissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BaseAmount", "CommissionRate", "EarnedDate", "IsPaid", "UserEmail", "UserId", "UserName" },
                values: new object[] { 0m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "", 0, "" });

            migrationBuilder.UpdateData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BaseAmount", "CommissionRate", "EarnedDate", "IsPaid", "UserEmail", "UserId", "UserName" },
                values: new object[] { 0m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "", 0, "" });

            migrationBuilder.UpdateData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BaseAmount", "CommissionRate", "EarnedDate", "IsPaid", "UserEmail", "UserId", "UserName" },
                values: new object[] { 0m, 0m, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "", 0, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "EarnedDate",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Commissions");
        }
    }
}
