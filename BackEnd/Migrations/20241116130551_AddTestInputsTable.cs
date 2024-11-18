using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddTestInputsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "287e0719-c4b9-4a16-8fac-c2dfb6e99474");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3774a0d6-4b87-4795-9cd1-9f51ec29eaff");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9d608f48-1aeb-43c3-afd4-bc0921df7c7f", null, "User", "USER" },
                    { "a7a0c021-853d-4516-9d0a-da930479b0f8", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "321946a0-3050-415a-bc2c-40e783474778", "e03cea08-e73d-47ef-b312-46f432c7592c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7c814a15-8b53-4f49-bf30-d03a7083bbd1", "00a3f6f8-49d2-46b7-96e6-2ef98e957284" });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "CompanyId", "Description", "Name", "Price", "TaxPercent" },
                values: new object[,]
                {
                    { 3, 1, "Description3", "Product3", 300, 22.0 },
                    { 4, 1, "Description4", "Product4", 400, 22.0 },
                    { 5, 1, "Description5", "Product5", 500, 22.0 },
                    { 6, 1, "Description6", "Product6", 600, 22.0 },
                    { 7, 1, "Description7", "Product7", 700, 22.0 },
                    { 8, 1, "Description9", "Product8", 800, 22.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d608f48-1aeb-43c3-afd4-bc0921df7c7f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7a0c021-853d-4516-9d0a-da930479b0f8");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "287e0719-c4b9-4a16-8fac-c2dfb6e99474", null, "Admin", "ADMIN" },
                    { "3774a0d6-4b87-4795-9cd1-9f51ec29eaff", null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f5d8595c-559f-4310-8899-1a9873790967", "0f9b1927-a704-444e-920b-de25f1e0998a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cb43543d-bcb9-4a2e-af7f-6f459de5c477", "fb588fb2-c899-466b-a070-5e774cfeb524" });
        }
    }
}
