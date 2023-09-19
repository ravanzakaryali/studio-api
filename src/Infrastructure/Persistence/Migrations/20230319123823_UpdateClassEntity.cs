using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class UpdateClassEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WitrinDate",
                table: "Classes",
                newName: "VitrinDate");

            migrationBuilder.AddColumn<Guid>(
                name: "Key",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "KeyExpirerDate",
                table: "Workers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "KeyExpirerDate",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "VitrinDate",
                table: "Classes",
                newName: "WitrinDate");
        }
    }
}
