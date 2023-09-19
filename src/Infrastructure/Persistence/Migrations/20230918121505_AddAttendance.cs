using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class AddAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSessions_Workers_WorkerId",
                table: "ClassSessions");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClassSessions_WorkerId",
            //    table: "ClassSessions");

            migrationBuilder.DropColumn(
                name: "People",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "ClassSessions");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "RoomSchedules",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "RoomSchedules",
                newName: "EndTime");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AttendancesWorkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalAttendanceHours = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancesWorkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendancesWorkers_ClassSessions_ClassSessionId",
                        column: x => x.ClassSessionId,
                        principalTable: "ClassSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendancesWorkers_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationWorker",
                columns: table => new
                {
                    ReservationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationWorker", x => new { x.ReservationsId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_ReservationWorker_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationWorker_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesWorkers_ClassSessionId",
                table: "AttendancesWorkers",
                column: "ClassSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendancesWorkers_WorkerId",
                table: "AttendancesWorkers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationWorker_WorkersId",
                table: "ReservationWorker",
                column: "WorkersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendancesWorkers");

            migrationBuilder.DropTable(
                name: "ReservationWorker");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "RoomSchedules",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "RoomSchedules",
                newName: "EndDate");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "People",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkerId",
                table: "ClassSessions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSessions_WorkerId",
                table: "ClassSessions",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSessions_Workers_WorkerId",
                table: "ClassSessions",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id");
        }
    }
}
