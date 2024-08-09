using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class ModifiedClassModuleWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkerType",
                table: "ClassModulesWorkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Teacher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkerType",
                table: "ClassModulesWorkers");
        }
    }
}
