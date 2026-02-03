using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingPlatformAPI.Migrations
{
    /// <inheritdoc />
    public partial class withdraws : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clicks_Affiliates_AffiliateId",
                table: "Clicks");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_Agents_AgentId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Agents_AgentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawals_Agents_AgentId",
                table: "Withdrawals");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Withdrawals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedDate",
                table: "Withdrawals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Withdrawals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedDate",
                table: "Withdrawals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Withdrawals",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Notes", "ProcessedDate", "RejectionReason", "RequestedDate" },
                values: new object[] { null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Withdrawals",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Notes", "ProcessedDate", "RejectionReason", "RequestedDate" },
                values: new object[] { null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_UserId",
                table: "Commissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clicks_Affiliates_AffiliateId",
                table: "Clicks",
                column: "AffiliateId",
                principalTable: "Affiliates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_Agents_AgentId",
                table: "Commissions",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_Users_UserId",
                table: "Commissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Agents_AgentId",
                table: "Users",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Withdrawals_Agents_AgentId",
                table: "Withdrawals",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clicks_Affiliates_AffiliateId",
                table: "Clicks");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_Agents_AgentId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_Users_UserId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Agents_AgentId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdrawals_Agents_AgentId",
                table: "Withdrawals");

            migrationBuilder.DropIndex(
                name: "IX_Commissions_UserId",
                table: "Commissions");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Withdrawals");

            migrationBuilder.DropColumn(
                name: "RequestedDate",
                table: "Withdrawals");

            migrationBuilder.AddForeignKey(
                name: "FK_Clicks_Affiliates_AffiliateId",
                table: "Clicks",
                column: "AffiliateId",
                principalTable: "Affiliates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_Agents_AgentId",
                table: "Commissions",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Agents_AgentId",
                table: "Users",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Withdrawals_Agents_AgentId",
                table: "Withdrawals",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
