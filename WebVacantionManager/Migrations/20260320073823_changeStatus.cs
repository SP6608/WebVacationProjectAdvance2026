using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebVacantionManager.Migrations
{
    /// <inheritdoc />
    public partial class changeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "VacationRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VacationRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f1a1111-1111-1111-1111-111111111111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0675c7b-af5c-41e4-9577-5cbf56d14824", "AQAAAAIAAYagAAAAENYLJ0CuuF9UDibEGEucoXLO76qfKRKuLEbQKTzdC6sjdjqk/ZC7foAsCZZvx6QxUQ==", "e3528d6b-2c5e-4cf6-92b3-9157bed3fe83" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2f2b2222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1ceae6f3-447f-494e-897f-7325f3baa936", "AQAAAAIAAYagAAAAEIqF4X4qh7zsWmEO8qYlOWw11GRvHEIlPEYF773RsqbpkZzyVqWw5wr3gnpPdcgr2g==", "f96d5c08-efb7-492d-8d34-746980c09a5c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f3c3333-3333-3333-3333-333333333333",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "047d30e6-6dea-4531-8aec-5c3fd81dcc18", "AQAAAAIAAYagAAAAEBvxzEtMTsdPsnXQ2t/ALdlO07H1JDeQVMUPqbde3gYSfoRJtMarxGh9b8Hq3I41QQ==", "5902cf20-7d57-47ed-9e61-5e303df43e17" });

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "VacationRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "VacationRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1f1a1111-1111-1111-1111-111111111111",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67658fab-3326-4b74-875a-62b75e6ab1de", "AQAAAAIAAYagAAAAEOmkoGB1TJ+Dce4kkhbW8cWdaSneJp7VwU4H4MLP7uYhBsMa1ZGWos6fU+59VS38Tw==", "9bd84838-8797-4890-8486-835d75a855d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2f2b2222-2222-2222-2222-222222222222",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a77cf355-120b-4a78-80cc-15d7f766e38f", "AQAAAAIAAYagAAAAEMbiJVk9fJQsJ6fV8TzSy89cC9w4yWNXV+bLEQbXHC2jUUb5+D9YYxxQiPi1H+YuQw==", "75f85684-32eb-49bb-a653-7ff9ea041f06" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3f3c3333-3333-3333-3333-333333333333",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7936acd-87b5-473f-b62b-428540187062", "AQAAAAIAAYagAAAAEDVY7B17VbYKWhK5kW0jXmnI0yeM71Am+8Jo99aeBRvc1vqjWZGubMKqT7p7QjbiuA==", "dc1199ff-17af-4ffa-a58b-e06cd3775595" });

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsApproved",
                value: true);

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsApproved",
                value: true);

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsApproved",
                value: true);

            migrationBuilder.UpdateData(
                table: "VacationRequests",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsApproved",
                value: false);
        }
    }
}
