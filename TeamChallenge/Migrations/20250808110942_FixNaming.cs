using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChallenge.Migrations
{
    /// <inheritdoc />
    public partial class FixNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_сosmetiс_СosmetiсId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_сosmetiс_OrderItem_OrderItemId",
                table: "сosmetiс");

            migrationBuilder.DropPrimaryKey(
                name: "PK_сosmetiс",
                table: "сosmetiс");

            migrationBuilder.RenameTable(
                name: "сosmetiс",
                newName: "Cosmetiс");

            migrationBuilder.RenameIndex(
                name: "IX_сosmetiс_OrderItemId",
                table: "Cosmetiс",
                newName: "IX_Cosmetiс_OrderItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cosmetiс",
                table: "Cosmetiс",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Cosmetiс_СosmetiсId",
                table: "Category",
                column: "СosmetiсId",
                principalTable: "Cosmetiс",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cosmetiс_OrderItem_OrderItemId",
                table: "Cosmetiс",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Cosmetiс_СosmetiсId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Cosmetiс_OrderItem_OrderItemId",
                table: "Cosmetiс");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cosmetiс",
                table: "Cosmetiс");

            migrationBuilder.RenameTable(
                name: "Cosmetiс",
                newName: "сosmetiс");

            migrationBuilder.RenameIndex(
                name: "IX_Cosmetiс_OrderItemId",
                table: "сosmetiс",
                newName: "IX_сosmetiс_OrderItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_сosmetiс",
                table: "сosmetiс",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_сosmetiс_СosmetiсId",
                table: "Category",
                column: "СosmetiсId",
                principalTable: "сosmetiс",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_сosmetiс_OrderItem_OrderItemId",
                table: "сosmetiс",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id");
        }
    }
}
