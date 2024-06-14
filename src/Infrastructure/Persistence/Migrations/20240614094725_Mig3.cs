using System;
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

            migrationBuilder.AddColumn<string>(
                name: "AvatarColor",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Supports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Supports",
                type: "nvarchar(max)",
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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PermissionLevels",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PermissionLevels",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PermissionLevels",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "Getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationModules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ApplicationModules",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ApplicationModules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerPermissionLevelAppModules",
                table: "WorkerPermissionLevelAppModules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionGroupPermissionLevelAppModules",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUserId = table.Column<int>(type: "int", nullable: true),
                    ToUserId = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupportCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Redirect = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Supports_ClassId",
                table: "Supports",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Supports_SupportCategoryId",
                table: "Supports",
                column: "SupportCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FromUserId",
                table: "Notifications",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ToUserId",
                table: "Notifications",
                column: "ToUserId");

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
                name: "FK_Supports_Classes_ClassId",
                table: "Supports",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supports_SupportCategories_SupportCategoryId",
                table: "Supports",
                column: "SupportCategoryId",
                principalTable: "SupportCategories",
                principalColumn: "Id");

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
                name: "FK_Supports_Classes_ClassId",
                table: "Supports");

            migrationBuilder.DropForeignKey(
                name: "FK_Supports_SupportCategories_SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerPermissionLevelAppModules_Workers_WorkerId",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SupportCategories");

            migrationBuilder.DropIndex(
                name: "IX_Supports_ClassId",
                table: "Supports");

            migrationBuilder.DropIndex(
                name: "IX_Supports_SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerPermissionLevelAppModules",
                table: "WorkerPermissionLevelAppModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionGroupPermissionLevelAppModules",
                table: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.DropColumn(
                name: "AvatarColor",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "SupportCategoryId",
                table: "Supports");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ApplicationModules");

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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PermissionLevels",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "PermissionLevels",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PermissionLevels",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "Getutcdate()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationModules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ApplicationModules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

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
