using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class ChangeAttendanceWorkers : Migration
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

            migrationBuilder.DropColumn(
                name: "VitrinDate",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "TotalAttendanceHours",
                table: "AttendancesWorkers",
                newName: "TotalMinutes");

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
                newName: "ClassTimeSheetsId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "SessionDetails",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "SessionDetails",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "SessionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Theoric");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "ClassModulesWorkers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Classes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttendanceStatus",
                table: "AttendancesWorkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalHours",
                table: "AttendancesWorkers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassGenerateSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "ClassGenerateSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalHours = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ClassGenerateSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassGenerateSessions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassGenerateSessions_ClassTimeSheets_ClassTimeSheetId",
                        column: x => x.ClassTimeSheetId,
                        principalTable: "ClassTimeSheets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassGenerateSessions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HeldModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassTimeSheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_HeldModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeldModules_ClassTimeSheets_ClassTimeSheetId",
                        column: x => x.ClassTimeSheetId,
                        principalTable: "ClassTimeSheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeldModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassGenerateSessions_ClassId",
                table: "ClassGenerateSessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGenerateSessions_ClassTimeSheetId",
                table: "ClassGenerateSessions",
                column: "ClassTimeSheetId",
                unique: true,
                filter: "[ClassTimeSheetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGenerateSessions_RoomId",
                table: "ClassGenerateSessions",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassTimeSheets_ClassId",
                table: "ClassTimeSheets",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_HeldModules_ClassTimeSheetId",
                table: "HeldModules",
                column: "ClassTimeSheetId");

            migrationBuilder.CreateIndex(
                name: "IX_HeldModules_ModuleId",
                table: "HeldModules",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetsId",
                table: "Attendances",
                column: "ClassTimeSheetsId",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ClassTimeSheets_ClassTimeSheetsId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_AttendancesWorkers_ClassTimeSheets_ClassTimeSheetId",
                table: "AttendancesWorkers");

            migrationBuilder.DropTable(
                name: "ClassGenerateSessions");

            migrationBuilder.DropTable(
                name: "HeldModules");

            migrationBuilder.DropTable(
                name: "ClassTimeSheets");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "SessionDetails");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "ClassModulesWorkers");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ClassModulesWorkers");

            migrationBuilder.DropColumn(
                name: "AttendanceStatus",
                table: "AttendancesWorkers");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "AttendancesWorkers");

            migrationBuilder.RenameColumn(
                name: "TotalMinutes",
                table: "AttendancesWorkers",
                newName: "TotalAttendanceHours");

            migrationBuilder.RenameColumn(
                name: "ClassTimeSheetId",
                table: "AttendancesWorkers",
                newName: "ClassSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_AttendancesWorkers_ClassTimeSheetId",
                table: "AttendancesWorkers",
                newName: "IX_AttendancesWorkers_ClassSessionId");

            migrationBuilder.RenameColumn(
                name: "ClassTimeSheetsId",
                table: "Attendances",
                newName: "ClassSessionId");


            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "SessionDetails",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "SessionDetails",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Classes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "VitrinDate",
                table: "Classes",
                type: "datetime2",
                nullable: true);

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
