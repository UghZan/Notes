using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.Main.Migrations
{
    /// <inheritdoc />
    public partial class EditNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastEditDate",
                table: "Notes",
                type: "datetime",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastEditDate",
                table: "Notes");
        }
    }
}
