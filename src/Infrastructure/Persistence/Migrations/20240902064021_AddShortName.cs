using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class AddShortName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "No",
                table: "Sessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Programs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VitrinDate",
                table: "Classes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VitrinWeek",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ProjectId",
                table: "Classes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Projects_ProjectId",
                table: "Classes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Projects_ProjectId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ProjectId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "No",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "VitrinDate",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "VitrinWeek",
                table: "Classes");
        }
    }
}
