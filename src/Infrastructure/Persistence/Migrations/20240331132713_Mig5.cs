using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class Mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Applications_ParentModuleId",
                table: "Applications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Applications",
                table: "Applications");

            migrationBuilder.RenameTable(
                name: "Applications",
                newName: "ApplicationModules");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_ParentModuleId",
                table: "ApplicationModules",
                newName: "IX_ApplicationModules_ParentModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationModules",
                table: "ApplicationModules",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApplicationModulePermissionGroup",
                columns: table => new
                {
                    ApplicationModulesId = table.Column<int>(type: "int", nullable: false),
                    PermissionGroupsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationModulePermissionGroup", x => new { x.ApplicationModulesId, x.PermissionGroupsId });
                    table.ForeignKey(
                        name: "FK_ApplicationModulePermissionGroup_ApplicationModules_ApplicationModulesId",
                        column: x => x.ApplicationModulesId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationModulePermissionGroup_PermissionGroups_PermissionGroupsId",
                        column: x => x.PermissionGroupsId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationModulePermissionLevel",
                columns: table => new
                {
                    ApplicationModulesId = table.Column<int>(type: "int", nullable: false),
                    PermissionLevelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationModulePermissionLevel", x => new { x.ApplicationModulesId, x.PermissionLevelsId });
                    table.ForeignKey(
                        name: "FK_ApplicationModulePermissionLevel_ApplicationModules_ApplicationModulesId",
                        column: x => x.ApplicationModulesId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationModulePermissionLevel_PermissionLevels_PermissionLevelsId",
                        column: x => x.PermissionLevelsId,
                        principalTable: "PermissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationModuleWorker",
                columns: table => new
                {
                    ApplicationModulesId = table.Column<int>(type: "int", nullable: false),
                    WorkersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationModuleWorker", x => new { x.ApplicationModulesId, x.WorkersId });
                    table.ForeignKey(
                        name: "FK_ApplicationModuleWorker_ApplicationModules_ApplicationModulesId",
                        column: x => x.ApplicationModulesId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationModuleWorker_Workers_WorkersId",
                        column: x => x.WorkersId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationModulePermissionGroup_PermissionGroupsId",
                table: "ApplicationModulePermissionGroup",
                column: "PermissionGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationModulePermissionLevel_PermissionLevelsId",
                table: "ApplicationModulePermissionLevel",
                column: "PermissionLevelsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationModuleWorker_WorkersId",
                table: "ApplicationModuleWorker",
                column: "WorkersId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointDetails_ApplicationModuleId",
                table: "EndpointDetails",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointDetails_PermissionAccessId",
                table: "EndpointDetails",
                column: "PermissionAccessId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationModules_ApplicationModules_ParentModuleId",
                table: "ApplicationModules",
                column: "ParentModuleId",
                principalTable: "ApplicationModules",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationModules_ApplicationModules_ParentModuleId",
                table: "ApplicationModules");

            migrationBuilder.DropTable(
                name: "ApplicationModulePermissionGroup");

            migrationBuilder.DropTable(
                name: "ApplicationModulePermissionLevel");

            migrationBuilder.DropTable(
                name: "ApplicationModuleWorker");

            migrationBuilder.DropTable(
                name: "EndpointDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationModules",
                table: "ApplicationModules");

            migrationBuilder.RenameTable(
                name: "ApplicationModules",
                newName: "Applications");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationModules_ParentModuleId",
                table: "Applications",
                newName: "IX_Applications_ParentModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Applications",
                table: "Applications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Applications_ParentModuleId",
                table: "Applications",
                column: "ParentModuleId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
