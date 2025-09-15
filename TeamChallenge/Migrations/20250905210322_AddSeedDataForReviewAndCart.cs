using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeamChallenge.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForReviewAndCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "UserId" },
                values: new object[] { 1, "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e" });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "ProductId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1, "Great product!", 1, 5, "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e" },
                    { 2, "Good value for money.", 1, 4, "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e" },
                    { 3, "Average quality.", 2, 3, "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
