using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class Mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModule_ApplicationModules_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModule_PermissionGroups_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModule_PermissionLevels_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModule_ApplicationModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModule_PermissionLevels_PermissionLevelId",
                table: "WorkerPermissionLevelAppModule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModule_Workers_WorkerId",
                table: "WorkerPermissionLevelAppModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerPermissionLevelAppModule",
                table: "WorkerPermissionLevelAppModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionGroupPermissionLevelAppModule",
                table: "PermissionGroupPermissionLevelAppModule");

            migrationBuilder.RenameTable(
                name: "WorkerPermissionLevelAppModule",
                newName: "WorkerPermissionLevelAppModules");

            migrationBuilder.RenameTable(
                name: "PermissionGroupPermissionLevelAppModule",
                newName: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerPermissionLevelAppModule_WorkerId",
                table: "WorkerPermissionLevelAppModules",
                newName: "IX_WorkerPermissionLevelAppModules_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerPermissionLevelAppModule_PermissionLevelId",
                table: "WorkerPermissionLevelAppModules",
                newName: "IX_WorkerPermissionLevelAppModules_PermissionLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerPermissionLevelAppModule_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModules",
                newName: "IX_WorkerPermissionLevelAppModules_ApplicationModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionGroupPermissionLevelAppModule_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModules",
                newName: "IX_PermissionGroupPermissionLevelAppModules_PermissionLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionGroupPermissionLevelAppModule_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModules",
                newName: "IX_PermissionGroupPermissionLevelAppModules_PermissionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionGroupPermissionLevelAppModule_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModules",
                newName: "IX_PermissionGroupPermissionLevelAppModules_ApplicationModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerPermissionLevelAppModules",
                table: "WorkerPermissionLevelAppModules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionGroupPermissionLevelAppModules",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "ApplicationModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModules_PermissionGroups_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "PermissionGroupId",
                principalTable: "PermissionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "PermissionLevelId",
                principalTable: "PermissionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModules",
                column: "ApplicationModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                table: "WorkerPermissionLevelAppModules",
                column: "PermissionLevelId",
                principalTable: "PermissionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_Workers_WorkerId",
                table: "WorkerPermissionLevelAppModules",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModules_PermissionGroups_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_Workers_WorkerId",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerPermissionLevelAppModules",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionGroupPermissionLevelAppModules",
                table: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.RenameTable(
                name: "WorkerPermissionLevelAppModules",
                newName: "WorkerPermissionLevelAppModule");

            migrationBuilder.RenameTable(
                name: "PermissionGroupPermissionLevelAppModules",
                newName: "PermissionGroupPermissionLevelAppModule");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerPermissionLevelAppModules_WorkerId",
                table: "WorkerPermissionLevelAppModule",
                newName: "IX_WorkerPermissionLevelAppModule_WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerPermissionLevelAppModules_PermissionLevelId",
                table: "WorkerPermissionLevelAppModule",
                newName: "IX_WorkerPermissionLevelAppModule_PermissionLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerPermissionLevelAppModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModule",
                newName: "IX_WorkerPermissionLevelAppModule_ApplicationModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionGroupPermissionLevelAppModules_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModule",
                newName: "IX_PermissionGroupPermissionLevelAppModule_PermissionLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionGroupPermissionLevelAppModules_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModule",
                newName: "IX_PermissionGroupPermissionLevelAppModule_PermissionGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_PermissionGroupPermissionLevelAppModules_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModule",
                newName: "IX_PermissionGroupPermissionLevelAppModule_ApplicationModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerPermissionLevelAppModule",
                table: "WorkerPermissionLevelAppModule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionGroupPermissionLevelAppModule",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModule_ApplicationModules_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "ApplicationModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModule_PermissionGroups_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "PermissionGroupId",
                principalTable: "PermissionGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionGroupPermissionLevelAppModule_PermissionLevels_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "PermissionLevelId",
                principalTable: "PermissionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPermissionLevelAppModule_ApplicationModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModule",
                column: "ApplicationModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPermissionLevelAppModule_PermissionLevels_PermissionLevelId",
                table: "WorkerPermissionLevelAppModule",
                column: "PermissionLevelId",
                principalTable: "PermissionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerPermissionLevelAppModule_Workers_WorkerId",
                table: "WorkerPermissionLevelAppModule",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
