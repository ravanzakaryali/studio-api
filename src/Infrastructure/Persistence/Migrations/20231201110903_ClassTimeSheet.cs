using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class ClassTimeSheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_GenerateClassSessions_GenerateClassSessionId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "ClassTimeSheetHeldModule");

            migrationBuilder.DropTable(
                name: "GenerateClassSessions");

            migrationBuilder.DropIndex(
                name: "IX_Workers_GenerateClassSessionId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "GenerateClassSessionId",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "ClassTimeSheetId",
                table: "Attendances",
                newName: "ClassTimeSheetsId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Attendances_ClassTimeSheetId",
            //    table: "Attendances",
            //    newName: "IX_Attendances_ClassTimeSheetsId");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassTimeSheetId",
                table: "HeldModule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "ClassTimeSheets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "Convert(date, getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "Convert(date, getdate())");

            migrationBuilder.CreateTable(
                name: "ClassSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalHours = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Offline"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false),
                    ClassTimeSheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSessions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessions_ClassTimeSheets_ClassTimeSheetId",
                        column: x => x.ClassTimeSheetId,
                        principalTable: "ClassTimeSheets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassSessions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSessions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendingWorkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendingWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendingWorkers_ClassSessions_ClassSessionId",
                        column: x => x.ClassSessionId,
                        principalTable: "ClassSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendingWorkers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendingWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeldModule_ClassTimeSheetId",
                table: "HeldModule",
                column: "ClassTimeSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTimeSheets_ModuleId",
                table: "ClassTimeSheets",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendingWorkers_ClassSessionId",
                table: "AttendingWorkers",
                column: "ClassSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendingWorkers_RoleId",
                table: "AttendingWorkers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendingWorkers_WorkerId",
                table: "AttendingWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_ClassId",
                table: "ClassSessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_ClassTimeSheetId",
                table: "ClassSessions",
                column: "ClassTimeSheetId",
                unique: true,
                filter: "[ClassTimeSheetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_ModuleId",
                table: "ClassSessions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_RoomId",
                table: "ClassSessions",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetsId",
                table: "Attendances",
                column: "ClassTimeSheetsId",
                principalTable: "ClassTimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassTimeSheets_Modules_ModuleId",
                table: "ClassTimeSheets",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeldModule_ClassTimeSheets_ClassTimeSheetId",
                table: "HeldModule",
                column: "ClassTimeSheetId",
                principalTable: "ClassTimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetsId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassTimeSheets_Modules_ModuleId",
                table: "ClassTimeSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_HeldModule_ClassTimeSheets_ClassTimeSheetId",
                table: "HeldModule");

            migrationBuilder.DropTable(
                name: "AttendingWorkers");

            migrationBuilder.DropTable(
                name: "ClassSessions");

            migrationBuilder.DropIndex(
                name: "IX_HeldModule_ClassTimeSheetId",
                table: "HeldModule");

            migrationBuilder.DropIndex(
                name: "IX_ClassTimeSheets_ModuleId",
                table: "ClassTimeSheets");

            migrationBuilder.DropColumn(
                name: "ClassTimeSheetId",
                table: "HeldModule");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "ClassTimeSheets");

            migrationBuilder.RenameColumn(
                name: "ClassTimeSheetsId",
                table: "Attendances",
                newName: "ClassTimeSheetId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Attendances_ClassTimeSheetsId",
            //    table: "Attendances",
            //    newName: "IX_Attendances_ClassTimeSheetId");

            migrationBuilder.AddColumn<Guid>(
                name: "GenerateClassSessionId",
                table: "Workers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "Convert(date, getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "Convert(date, getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ClassTimeSheetHeldModule",
                columns: table => new
                {
                    ClassTimeSheetsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeldModulesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTimeSheetHeldModule", x => new { x.ClassTimeSheetsId, x.HeldModulesId });
                    table.ForeignKey(
                        name: "FK_ClassTimeSheetHeldModule_ClassTimeSheets_ClassTimeSheetsId",
                        column: x => x.ClassTimeSheetsId,
                        principalTable: "ClassTimeSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassTimeSheetHeldModule_HeldModule_HeldModulesId",
                        column: x => x.HeldModulesId,
                        principalTable: "HeldModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenerateClassSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassTimeSheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Offline"),
                    TotalHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerateClassSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenerateClassSessions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenerateClassSessions_ClassTimeSheets_ClassTimeSheetId",
                        column: x => x.ClassTimeSheetId,
                        principalTable: "ClassTimeSheets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GenerateClassSessions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenerateClassSessions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_GenerateClassSessionId",
                table: "Workers",
                column: "GenerateClassSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTimeSheetHeldModule_HeldModulesId",
                table: "ClassTimeSheetHeldModule",
                column: "HeldModulesId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerateClassSessions_ClassId",
                table: "GenerateClassSessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerateClassSessions_ClassTimeSheetId",
                table: "GenerateClassSessions",
                column: "ClassTimeSheetId",
                unique: true,
                filter: "[ClassTimeSheetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GenerateClassSessions_ModuleId",
                table: "GenerateClassSessions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerateClassSessions_RoomId",
                table: "GenerateClassSessions",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetId",
                table: "Attendances",
                column: "ClassTimeSheetId",
                principalTable: "ClassTimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_GenerateClassSessions_GenerateClassSessionId",
                table: "Workers",
                column: "GenerateClassSessionId",
                principalTable: "GenerateClassSessions",
                principalColumn: "Id");
        }
    }
}
