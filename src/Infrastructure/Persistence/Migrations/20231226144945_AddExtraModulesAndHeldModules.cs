using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class AddExtraModulesAndHeldModules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeldModules_Modules_ModuleId",
                table: "HeldModules");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "HeldModules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExtraModuleId",
                table: "HeldModules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExtraModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hours = table.Column<double>(type: "float", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraModules_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassExtraModulesWorkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    ExtraModuleId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassExtraModulesWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassExtraModulesWorkers_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassExtraModulesWorkers_ExtraModules_ExtraModuleId",
                        column: x => x.ExtraModuleId,
                        principalTable: "ExtraModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassExtraModulesWorkers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassExtraModulesWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeldModules_ExtraModuleId",
                table: "HeldModules",
                column: "ExtraModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtraModulesWorkers_ClassId",
                table: "ClassExtraModulesWorkers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtraModulesWorkers_ExtraModuleId",
                table: "ClassExtraModulesWorkers",
                column: "ExtraModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtraModulesWorkers_RoleId",
                table: "ClassExtraModulesWorkers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassExtraModulesWorkers_WorkerId",
                table: "ClassExtraModulesWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraModules_ProgramId",
                table: "ExtraModules",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeldModules_ExtraModules_ExtraModuleId",
                table: "HeldModules",
                column: "ExtraModuleId",
                principalTable: "ExtraModules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeldModules_Modules_ModuleId",
                table: "HeldModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeldModules_ExtraModules_ExtraModuleId",
                table: "HeldModules");

            migrationBuilder.DropForeignKey(
                name: "FK_HeldModules_Modules_ModuleId",
                table: "HeldModules");

            migrationBuilder.DropTable(
                name: "ClassExtraModulesWorkers");

            migrationBuilder.DropTable(
                name: "ExtraModules");

            migrationBuilder.DropIndex(
                name: "IX_HeldModules_ExtraModuleId",
                table: "HeldModules");

            migrationBuilder.DropColumn(
                name: "ExtraModuleId",
                table: "HeldModules");

            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "HeldModules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HeldModules_Modules_ModuleId",
                table: "HeldModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
