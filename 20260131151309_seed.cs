using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GamingPlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Affiliates",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { 1, "affiliate@example.com", "SuperAffiliate", "hashedpassword" });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "Id", "Email", "Name", "PasswordHash" },
                values: new object[] { 1, "john@example.com", "John Doe", "hashedpassword" });

            migrationBuilder.InsertData(
                table: "Clicks",
                columns: new[] { "Id", "AffiliateId", "IPAddress", "Timestamp" },
                values: new object[,]
                {
                    { 1, 1, "192.168.1.100", new DateTime(2026, 1, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "192.168.1.101", new DateTime(2026, 1, 31, 12, 5, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Commissions",
                columns: new[] { "Id", "AgentId", "Amount", "Date" },
                values: new object[,]
                {
                    { 1, 1, 100.50m, new DateTime(2026, 1, 31, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 200.75m, new DateTime(2026, 1, 30, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, 150.00m, new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AgentId", "Email", "FullName", "IsBlocked", "PasswordHash" },
                values: new object[] { 1, 1, "alice@example.com", "Alice Johnson", false, "hashedpassword" });

            migrationBuilder.InsertData(
                table: "Withdrawals",
                columns: new[] { "Id", "AgentId", "Amount", "RequestDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, 50.00m, new DateTime(2026, 1, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), "Pending" },
                    { 2, 1, 100.00m, new DateTime(2026, 1, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), "Approved" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clicks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clicks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Commissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Withdrawals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Withdrawals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Affiliates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
