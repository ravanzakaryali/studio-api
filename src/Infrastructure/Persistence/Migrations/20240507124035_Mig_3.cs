using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class Mig_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportCategory_SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportCategory",
                table: "SupportCategory");

            migrationBuilder.RenameTable(
                name: "SupportCategory",
                newName: "SupportCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportCategories",
                table: "SupportCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supports_SupportCategories_SupportCategoryId",
                table: "Supports",
                column: "SupportCategoryId",
                principalTable: "SupportCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportCategories_SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupportCategories",
                table: "SupportCategories");

            migrationBuilder.RenameTable(
                name: "SupportCategories",
                newName: "SupportCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupportCategory",
                table: "SupportCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supports_SupportCategory_SupportCategoryId",
                table: "Supports",
                column: "SupportCategoryId",
                principalTable: "SupportCategory",
                principalColumn: "Id");
        }
    }
}
