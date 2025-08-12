using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChallenge.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Cosmetiс_СosmetiсId",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "СosmetiсId",
                table: "Category",
                newName: "CosmeticId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_СosmetiсId",
                table: "Category",
                newName: "IX_Category_CosmeticId");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Cosmetiс_CosmeticId",
                table: "Category",
                column: "CosmeticId",
                principalTable: "Cosmetiс",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Cosmetiс_CosmeticId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CosmeticId",
                table: "Category",
                newName: "СosmetiсId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_CosmeticId",
                table: "Category",
                newName: "IX_Category_СosmetiсId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Cosmetiс_СosmetiсId",
                table: "Category",
                column: "СosmetiсId",
                principalTable: "Cosmetiс",
                principalColumn: "Id");
        }
    }
}
