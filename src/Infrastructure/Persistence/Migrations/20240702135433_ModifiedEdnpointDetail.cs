using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class ModifiedEdnpointDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndpointDetails_ApplicationModules_ApplicationModuleId",
                table: "EndpointDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationModuleId",
                table: "EndpointDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_EndpointDetails_ApplicationModules_ApplicationModuleId",
                table: "EndpointDetails",
                column: "ApplicationModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndpointDetails_ApplicationModules_ApplicationModuleId",
                table: "EndpointDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationModuleId",
                table: "EndpointDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EndpointDetails_ApplicationModules_ApplicationModuleId",
                table: "EndpointDetails",
                column: "ApplicationModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
