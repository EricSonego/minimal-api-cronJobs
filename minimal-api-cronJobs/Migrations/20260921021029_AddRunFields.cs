using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api_cronJobs.Migrations
{
    /// <inheritdoc />
    public partial class AddRunFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastRun",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRun",
                table: "Jobs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRun",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "NextRun",
                table: "Jobs");
        }
    }
}
