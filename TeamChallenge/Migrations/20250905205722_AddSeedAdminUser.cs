using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamChallenge.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SentEmailTime", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e", 0, "cdca885a-43f5-4929-85c5-9b41dd697b37", "admin@gmail.com", true, true, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEGX+x7oprDHdtrcw9g2r0B/J6Ae4IiS7/2HhEt4k6Zx7q3KtOmCXrvFrDxMlY8ox3A==", null, false, "V4WTZVKR2NZW2BOK4YAEARQOCJHSV4SK", new DateTime(2025, 9, 5, 23, 43, 34, 0, DateTimeKind.Unspecified), false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d4a7c4fb-a129-47ff-b520-df1e8799d609", "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d4a7c4fb-a129-47ff-b520-df1e8799d609", "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2e0e8d05-b3b5-4878-8a4b-e0db5ed4492e");
        }
    }
}
