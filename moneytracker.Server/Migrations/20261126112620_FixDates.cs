using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneytracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class FixDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_MemberId1",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_MemberId1",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_MemberId1",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_MemberId1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "MemberId1",
                table: "Categories");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "Subscriptions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "NextBillingDate",
                table: "Subscriptions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Subscriptions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "Subscriptions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Categories",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MemberId",
                table: "Subscriptions",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MemberId",
                table: "Categories",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_MemberId",
                table: "Categories",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_MemberId",
                table: "Subscriptions",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_MemberId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_MemberId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_MemberId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_MemberId",
                table: "Categories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextBillingDate",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Subscriptions",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberId1",
                table: "Subscriptions",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Categories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "MemberId1",
                table: "Categories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MemberId1",
                table: "Subscriptions",
                column: "MemberId1");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_MemberId1",
                table: "Categories",
                column: "MemberId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_MemberId1",
                table: "Categories",
                column: "MemberId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_MemberId1",
                table: "Subscriptions",
                column: "MemberId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
