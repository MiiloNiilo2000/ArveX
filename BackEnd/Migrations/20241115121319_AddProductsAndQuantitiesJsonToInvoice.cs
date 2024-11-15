using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsAndQuantitiesJsonToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f9974d5-1d5d-4757-a218-8778bd8fa3ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abbfb38f-1458-4255-859c-b07a332ddb94");

            migrationBuilder.DropColumn(
                name: "ProductIds",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "ProductsAndQuantitiesJson",
                table: "Invoice",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58759d26-7fe0-42d8-ba5d-ef6ad01b7fd9", null, "Admin", "ADMIN" },
                    { "673ffcc4-6bc4-42ed-80cf-70250c9637bb", null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b4c70667-4b79-4fc4-8799-f4b0906ee3f0", "fe6f10d5-63b8-4479-a026-b8a89e2f2a48" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "cc8bb003-c21f-4bee-9543-c8e2a9b6bb92", "fefe195a-6cdb-44f8-98d3-a8648dab069b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58759d26-7fe0-42d8-ba5d-ef6ad01b7fd9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "673ffcc4-6bc4-42ed-80cf-70250c9637bb");

            migrationBuilder.DropColumn(
                name: "ProductsAndQuantitiesJson",
                table: "Invoice");

            migrationBuilder.AddColumn<List<int>>(
                name: "ProductIds",
                table: "Invoice",
                type: "integer[]",
                nullable: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9f9974d5-1d5d-4757-a218-8778bd8fa3ba", null, "User", "USER" },
                    { "abbfb38f-1458-4255-859c-b07a332ddb94", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5d07f0fc-15cf-46f8-8301-dd697818db9a", "fe090450-bbb1-4d7a-9c21-9a4cd1005eb0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "12e08cee-b4ec-4a61-8f8e-b32a062b706f", "ed174aae-33af-4177-93f3-c94a56480725" });
        }
    }
}
