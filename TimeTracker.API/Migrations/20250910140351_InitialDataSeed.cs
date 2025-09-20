using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TimeTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Code", "Description" },
                values: new object[,]
                {
                    { 1, "BPC.001", "Berkshire Primary Care 001" },
                    { 2, "BP", "ARRS" }
                });

            migrationBuilder.InsertData(
                table: "SegmentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Meeting" },
                    { 2, "Calls" },
                    { 3, "Planning" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "UserName" },
                values: new object[,]
                {
                    { 1, "Kirstine Hall", "kirstine" },
                    { 2, "Toby Jones", "toby" }
                });

            migrationBuilder.InsertData(
                table: "TimeEntries",
                columns: new[] { "Id", "EndDateTime", "ProjectId", "SegmentTypeId", "StartDateTime", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 1, 17, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 8, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2025, 8, 2, 13, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, new DateTime(2025, 8, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, new DateTime(2025, 8, 2, 15, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, new DateTime(2025, 8, 2, 13, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TimeEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimeEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimeEntries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SegmentTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SegmentTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SegmentTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
