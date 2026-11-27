using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace moneytracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class OptionalCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Categories_CategoryId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Subscriptions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Categories_CategoryId",
                table: "Subscriptions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Categories_CategoryId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Subscriptions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Categories_CategoryId",
                table: "Subscriptions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
