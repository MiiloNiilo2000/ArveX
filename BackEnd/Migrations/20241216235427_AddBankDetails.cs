using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBankDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03a22643-aa50-4c56-b10c-9964d3337d22");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18c6358e-2170-4031-a38a-ae4bad8d4342");

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SWIFT",
                table: "Company",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8443c408-b999-4ca8-8bbb-51934ef7552e", null, "Admin", "ADMIN" },
                    { "fbaf5c31-f840-4809-aa89-704c88a80336", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8443c408-b999-4ca8-8bbb-51934ef7552e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbaf5c31-f840-4809-aa89-704c88a80336");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "SWIFT",
                table: "Company");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03a22643-aa50-4c56-b10c-9964d3337d22", null, "Admin", "ADMIN" },
                    { "18c6358e-2170-4031-a38a-ae4bad8d4342", null, "User", "USER" }
                });
        }
    }
}
