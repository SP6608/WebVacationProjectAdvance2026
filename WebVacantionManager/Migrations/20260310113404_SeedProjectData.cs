using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebVacantionManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedProjectData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Description", "ProjectName" },
                values: new object[,]
                {
                    { 3, "System for managing students, teachers and courses.", "School Management System" },
                    { 4, "Application for tracking tasks, deadlines and productivity.", "Task Tracker" },
                    { 5, "Web platform for booking tables and managing reservations.", "Restaurant Reservation System" },
                    { 6, "Application that tracks workouts, calories and progress.", "Fitness Tracker" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
