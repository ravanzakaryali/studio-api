using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class WitrinDateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WitrinDate",
                table: "Classes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WitrinDate",
                table: "Classes",
                type: "datetime2",
                nullable: true);
        }
    }
}
