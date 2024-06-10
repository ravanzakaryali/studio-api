using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class Mig_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles");

            // migrationBuilder.DropIndex(
            //     name: "IX_UserRoles_UserId",
            //     table: "UserRoles");

            // migrationBuilder.DropColumn(
            //     name: "Id",
            //     table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Supports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Supports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Open");

            migrationBuilder.AddColumn<int>(
                name: "SupportCategoryId",
                table: "Supports",
                type: "int",
                nullable: true);

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles",
            //     columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateTable(
                name: "SupportCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Supports_ClassId",
                table: "Supports",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Supports_SupportCategoryId",
                table: "Supports",
                column: "SupportCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supports_Classes_ClassId",
                table: "Supports",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supports_SupportCategory_SupportCategoryId",
                table: "Supports",
                column: "SupportCategoryId",
                principalTable: "SupportCategory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_Classes_ClassId",
                table: "Supports");

            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportCategory_SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropTable(
                name: "SupportCategory");

            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Supports_ClassId",
                table: "Supports");

            migrationBuilder.DropIndex(
                name: "IX_Supports_SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "SupportCategoryId",
                table: "Supports");

            // migrationBuilder.AddColumn<int>(
            //     name: "Id",
            //     table: "UserRoles",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0)
            //     .Annotation("SqlServer:Identity", "1, 1");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles",
            //     column: "Id");

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserRoles_UserId",
            //     table: "UserRoles",
            //     column: "UserId");
        }
    }
}
