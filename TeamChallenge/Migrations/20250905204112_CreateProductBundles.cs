using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChallenge.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductBundles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPrice",
                table: "Products",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductBundleId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductBundles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBundles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProductBundles",
                columns: new[] { "Id", "Description", "DiscountPrice", "Name", "Price", "StockQuantity" },
                values: new object[] { 1, "Description for product bundle 1", null, "Prod bundle 1", 90.99m, 10 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DiscountPrice", "ProductBundleId" },
                values: new object[] { null, 1 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscountPrice", "ProductBundleId" },
                values: new object[] { null, 1 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DiscountPrice", "ProductBundleId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductBundleId",
                table: "Products",
                column: "ProductBundleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBundles_ProductBundleId",
                table: "Products",
                column: "ProductBundleId",
                principalTable: "ProductBundles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBundles_ProductBundleId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductBundles");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductBundleId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductBundleId",
                table: "Products");
        }
    }
}
