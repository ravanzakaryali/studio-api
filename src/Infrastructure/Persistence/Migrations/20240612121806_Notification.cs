using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class Notification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllUsers",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "ToUserId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToUserId",
                table: "Notifications",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_ToUserId",
                table: "Notifications",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_ToUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ToUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "AllUsers",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
