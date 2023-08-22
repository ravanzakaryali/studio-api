using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class AddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "ClassModulesWorkers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassModulesWorkers_RoleId",
                table: "ClassModulesWorkers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassModulesWorkers_AspNetRoles_RoleId",
                table: "ClassModulesWorkers",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassModulesWorkers_AspNetRoles_RoleId",
                table: "ClassModulesWorkers");

            migrationBuilder.DropIndex(
                name: "IX_ClassModulesWorkers_RoleId",
                table: "ClassModulesWorkers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "ClassModulesWorkers");
        }
    }
}
