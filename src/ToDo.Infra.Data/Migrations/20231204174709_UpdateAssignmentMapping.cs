using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDo.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignmentMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "Assignments",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "Assignments",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }
    }
}
