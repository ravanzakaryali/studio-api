using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class ClassSessionsAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassSessions_ClassSessionId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendancesWorkers_ClassSessions_ClassSessionId",
                table: "AttendancesWorkers");

            migrationBuilder.DropTable(
                name: "ClassSessions");

            migrationBuilder.RenameColumn(
                name: "VitrinDate",
                table: "Classes",
                newName: "WitrinDate");

            migrationBuilder.RenameColumn(
                name: "ClassSessionId",
                table: "AttendancesWorkers",
                newName: "ClassTimeSheetId");

            migrationBuilder.RenameIndex(
                name: "IX_AttendancesWorkers_ClassSessionId",
                table: "AttendancesWorkers",
                newName: "IX_AttendancesWorkers_ClassTimeSheetId");

            migrationBuilder.RenameColumn(
                name: "ClassSessionId",
                table: "Attendances",
                newName: "ClassTimeSheetId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Attendances_ClassSessionId",
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
                nullable: true,
                defaultValueSql: "Convert(date, getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "Convert(date, getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Classes",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ClassTimeSheets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalHours = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassTimeSheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassTimeSheets_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeldModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalHours = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeldModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeldModule_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenerateClassSessions",
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

            migrationBuilder.CreateIndex(
                name: "IX_Workers_GenerateClassSessionId",
                table: "Workers",
                column: "GenerateClassSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTimeSheetHeldModule_HeldModulesId",
                table: "ClassTimeSheetHeldModule",
                column: "HeldModulesId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTimeSheets_ClassId",
                table: "ClassTimeSheets",
                column: "ClassId");

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

            migrationBuilder.CreateIndex(
                name: "IX_HeldModule_ModuleId",
                table: "HeldModule",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetId",
                table: "Attendances",
                column: "ClassTimeSheetId",
                principalTable: "ClassTimeSheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendancesWorkers_ClassTimeSheets_ClassTimeSheetId",
                table: "AttendancesWorkers",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendancesWorkers_ClassTimeSheets_ClassTimeSheetId",
                table: "AttendancesWorkers");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_GenerateClassSessions_GenerateClassSessionId",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "ClassTimeSheetHeldModule");

            migrationBuilder.DropTable(
                name: "GenerateClassSessions");

            migrationBuilder.DropTable(
                name: "HeldModule");

            migrationBuilder.DropTable(
                name: "ClassTimeSheets");

            migrationBuilder.DropIndex(
                name: "IX_Workers_GenerateClassSessionId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "GenerateClassSessionId",
                table: "Workers");

            migrationBuilder.RenameColumn(
                name: "WitrinDate",
                table: "Classes",
                newName: "VitrinDate");

            migrationBuilder.RenameColumn(
                name: "ClassTimeSheetId",
                table: "AttendancesWorkers",
                newName: "ClassSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttendancesWorkers_ClassTimeSheetId",
                table: "AttendancesWorkers",
                newName: "IX_AttendancesWorkers_ClassSessionId");

            migrationBuilder.RenameColumn(
                name: "ClassTimeSheetId",
                table: "Attendances",
                newName: "ClassSessionId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Attendances_ClassTimeSheetId",
            //    table: "Attendances",
            //    newName: "IX_Attendances_ClassSessionId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "Convert(date, getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "Convert(date, getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Classes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "ClassSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TotalHour = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_ClassSessions_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassSessions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_ClassId",
                table: "ClassSessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_ModuleId",
                table: "ClassSessions",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_RoomId",
                table: "ClassSessions",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassSessions_ClassSessionId",
                table: "Attendances",
                column: "ClassSessionId",
                principalTable: "ClassSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendancesWorkers_ClassSessions_ClassSessionId",
                table: "AttendancesWorkers",
                column: "ClassSessionId",
                principalTable: "ClassSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
