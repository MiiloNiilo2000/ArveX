using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_ProfileId",
                table: "Product");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ec0eee0-70ba-4814-8a74-2ee573a235bf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca8435bc-c85d-48e7-95bd-d0700aa7d67e");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Product",
                newName: "profileId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_ProfileId",
                table: "Product",
                newName: "IX_Product_profileId");

            migrationBuilder.AddColumn<string>(
                name: "SenderCompanyAddress",
                table: "PrivatePersonInvoice",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderCompanyKMKRNumber",
                table: "PrivatePersonInvoice",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderCompanyName",
                table: "PrivatePersonInvoice",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SenderCompanyRegistrationNumber",
                table: "PrivatePersonInvoice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0326832e-861d-4da0-bb22-cfa1cebc2b06", null, "Admin", "ADMIN" },
                    { "4475182e-e677-4baa-9e13-d44f31c6cd30", null, "User", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_profileId",
                table: "Product",
                column: "profileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_AspNetUsers_profileId",
                table: "Product");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0326832e-861d-4da0-bb22-cfa1cebc2b06");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4475182e-e677-4baa-9e13-d44f31c6cd30");

            migrationBuilder.DropColumn(
                name: "SenderCompanyAddress",
                table: "PrivatePersonInvoice");

            migrationBuilder.DropColumn(
                name: "SenderCompanyKMKRNumber",
                table: "PrivatePersonInvoice");

            migrationBuilder.DropColumn(
                name: "SenderCompanyName",
                table: "PrivatePersonInvoice");

            migrationBuilder.DropColumn(
                name: "SenderCompanyRegistrationNumber",
                table: "PrivatePersonInvoice");

            migrationBuilder.RenameColumn(
                name: "profileId",
                table: "Product",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_profileId",
                table: "Product",
                newName: "IX_Product_ProfileId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6ec0eee0-70ba-4814-8a74-2ee573a235bf", null, "User", "USER" },
                    { "ca8435bc-c85d-48e7-95bd-d0700aa7d67e", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_ProfileId",
                table: "Product",
                column: "ProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
