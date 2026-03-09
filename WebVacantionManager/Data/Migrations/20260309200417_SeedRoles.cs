using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebVacantionManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a310333-c4a5-4457-9065-a861e635d848", null, "Ceo", "CEO" },
                    { "96554597-d11d-48bb-84d5-bbf7442a7afc", null, "Developer", "DEVELOPER" },
                    { "dd504816-18f8-420f-a69f-ca3e6386ba5c", null, "TeamLead", "TEAMLEAD" },
                    { "e3233cd7-132a-45ca-8a32-7160411726ed", null, "Unassigned", "UNASSIGNED" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a310333-c4a5-4457-9065-a861e635d848");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96554597-d11d-48bb-84d5-bbf7442a7afc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd504816-18f8-420f-a69f-ca3e6386ba5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3233cd7-132a-45ca-8a32-7160411726ed");
        }
    }
}
