using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChallenge.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModelsForDeployment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
                column: "SentEmailTime",
                value: new DateTime(2025, 5, 9, 23, 43, 34, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e",
                column: "SentEmailTime",
                value: new DateTime(2025, 9, 5, 23, 43, 34, 0, DateTimeKind.Unspecified));
        }
    }
}
