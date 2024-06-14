using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles");

            // migrationBuilder.DropIndex(
            //     name: "IX_UserRoles_UserId",
            //     table: "UserRoles");

            // migrationBuilder.DropColumn(
            //     name: "Id",
            //     table: "UserRoles");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles",
            //     columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateTable(
                name: "ApplicationModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentModuleId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationModules_ApplicationModules_ParentModuleId",
                        column: x => x.ParentModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Endpoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HttpMethod = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "GET"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionAccesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EndpointDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermissionAccessId = table.Column<int>(type: "int", nullable: false),
                    ApplicationModuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointDetails_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EndpointDetails_Endpoints_Id",
                        column: x => x.Id,
                        principalTable: "Endpoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EndpointDetails_PermissionAccesses_PermissionAccessId",
                        column: x => x.PermissionAccessId,
                        principalTable: "PermissionAccesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroupWorker",
                columns: table => new
                {
                    PermissionGroupsId = table.Column<int>(type: "int", nullable: false),
                    WorkersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroupWorker", x => new { x.PermissionGroupsId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_PermissionGroupWorker_PermissionGroups_PermissionGroupsId",
                        column: x => x.PermissionGroupsId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupWorker_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionAccessPermissionLevel",
                columns: table => new
                {
                    PermissionAccessesId = table.Column<int>(type: "int", nullable: false),
                    PermissionLevelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionAccessPermissionLevel", x => new { x.PermissionAccessesId, x.PermissionLevelsId });
                    table.ForeignKey(
                        name: "FK_PermissionAccessPermissionLevel_PermissionAccesses_PermissionAccessesId",
                        column: x => x.PermissionAccessesId,
                        principalTable: "PermissionAccesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionAccessPermissionLevel_PermissionLevels_PermissionLevelsId",
                        column: x => x.PermissionLevelsId,
                        principalTable: "PermissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroupPermissionLevelAppModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationModuleId = table.Column<int>(type: "int", nullable: false),
                    PermissionLevelId = table.Column<int>(type: "int", nullable: false),
                    PermissionGroupId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroupPermissionLevelAppModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionGroupPermissionLevelAppModule_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupPermissionLevelAppModule_PermissionGroups_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupPermissionLevelAppModule_PermissionLevels_PermissionLevelId",
                        column: x => x.PermissionLevelId,
                        principalTable: "PermissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerPermissionLevelAppModule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationModuleId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    PermissionLevelId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerPermissionLevelAppModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerPermissionLevelAppModule_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerPermissionLevelAppModule_PermissionLevels_PermissionLevelId",
                        column: x => x.PermissionLevelId,
                        principalTable: "PermissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerPermissionLevelAppModule_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationModules_ParentModuleId",
                table: "ApplicationModules",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointDetails_ApplicationModuleId",
                table: "EndpointDetails",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointDetails_PermissionAccessId",
                table: "EndpointDetails",
                column: "PermissionAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionAccessPermissionLevel_PermissionLevelsId",
                table: "PermissionAccessPermissionLevel",
                column: "PermissionLevelsId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermissionLevelAppModule_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermissionLevelAppModule_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermissionLevelAppModule_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModule",
                column: "PermissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupWorker_WorkersId",
                table: "PermissionGroupWorker",
                column: "WorkersId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPermissionLevelAppModule_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModule",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPermissionLevelAppModule_PermissionLevelId",
                table: "WorkerPermissionLevelAppModule",
                column: "PermissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPermissionLevelAppModule_WorkerId",
                table: "WorkerPermissionLevelAppModule",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointDetails");

            migrationBuilder.DropTable(
                name: "PermissionAccessPermissionLevel");

            migrationBuilder.DropTable(
                name: "PermissionGroupPermissionLevelAppModule");

            migrationBuilder.DropTable(
                name: "PermissionGroupWorker");

            migrationBuilder.DropTable(
                name: "WorkerPermissionLevelAppModule");

            migrationBuilder.DropTable(
                name: "Endpoints");

            migrationBuilder.DropTable(
                name: "PermissionAccesses");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.DropTable(
                name: "ApplicationModules");

            migrationBuilder.DropTable(
                name: "PermissionLevels");

            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }
    }
}
