using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class AddedPermission : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "AvatarColor",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fincode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Yoxdur");

            // migrationBuilder.AddColumn<int>(
            //     name: "ClassId",
            //     table: "Supports",
            //     type: "int",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Note",
            //     table: "Supports",
            //     type: "nvarchar(max)",
            //     nullable: true);

            // migrationBuilder.AddColumn<string>(
            //     name: "Status",
            //     table: "Supports",
            //     type: "nvarchar(max)",
            //     nullable: false,
            //     defaultValue: "Open");

            // migrationBuilder.AddColumn<int>(
            //     name: "SupportCategoryId",
            //     table: "Supports",
            //     type: "int",
            //     nullable: true);

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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentModuleId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
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

            // migrationBuilder.CreateTable(
            //     name: "Notifications",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         FromUserId = table.Column<int>(type: "int", nullable: true),
            //         ToUserId = table.Column<int>(type: "int", nullable: true),
            //         IsRead = table.Column<bool>(type: "bit", nullable: false),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //         IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
            //         CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
            //         CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //         LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //         LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Notifications", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Notifications_Users_FromUserId",
            //             column: x => x.FromUserId,
            //             principalTable: "Users",
            //             principalColumn: "Id");
            //         table.ForeignKey(
            //             name: "FK_Notifications_Users_ToUserId",
            //             column: x => x.ToUserId,
            //             principalTable: "Users",
            //             principalColumn: "Id");
            //     });

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
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "Getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLevels", x => x.Id);
                });

            // migrationBuilder.CreateTable(
            //     name: "SupportCategories",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Redirect = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
            //         IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_SupportCategories", x => x.Id);
            //     });

            migrationBuilder.CreateTable(
                name: "EndpointAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndpointId = table.Column<int>(type: "int", nullable: false),
                    PermissionAccessId = table.Column<int>(type: "int", nullable: false),
                    ApplicationModuleId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointAccesses_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EndpointAccesses_Endpoints_EndpointId",
                        column: x => x.EndpointId,
                        principalTable: "Endpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EndpointAccesses_PermissionAccesses_PermissionAccessId",
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
                name: "PermissionGroupPermissionLevelAppModules",
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
                    table.PrimaryKey("PK_PermissionGroupPermissionLevelAppModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionGroupPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupPermissionLevelAppModules_PermissionGroups_PermissionGroupId",
                        column: x => x.PermissionGroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionGroupPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                        column: x => x.PermissionLevelId,
                        principalTable: "PermissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerPermissionLevelAppModules",
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
                    table.PrimaryKey("PK_WorkerPermissionLevelAppModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerPermissionLevelAppModules_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerPermissionLevelAppModules_PermissionLevels_PermissionLevelId",
                        column: x => x.PermissionLevelId,
                        principalTable: "PermissionLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerPermissionLevelAppModules_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Supports_ClassId",
            //     table: "Supports",
            //     column: "ClassId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Supports_SupportCategoryId",
            //     table: "Supports",
            //     column: "SupportCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationModules_NormalizedName",
                table: "ApplicationModules",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationModules_ParentModuleId",
                table: "ApplicationModules",
                column: "ParentModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointAccesses_ApplicationModuleId",
                table: "EndpointAccesses",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointAccesses_EndpointId",
                table: "EndpointAccesses",
                column: "EndpointId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointAccesses_PermissionAccessId",
                table: "EndpointAccesses",
                column: "PermissionAccessId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Notifications_FromUserId",
            //     table: "Notifications",
            //     column: "FromUserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Notifications_ToUserId",
            //     table: "Notifications",
            //     column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionAccessPermissionLevel_PermissionLevelsId",
                table: "PermissionAccessPermissionLevel",
                column: "PermissionLevelsId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermissionLevelAppModules_ApplicationModuleId",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermissionLevelAppModules_PermissionGroupId",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "PermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupPermissionLevelAppModules_PermissionLevelId",
                table: "PermissionGroupPermissionLevelAppModules",
                column: "PermissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroupWorker_WorkersId",
                table: "PermissionGroupWorker",
                column: "WorkersId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPermissionLevelAppModules_ApplicationModuleId",
                table: "WorkerPermissionLevelAppModules",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPermissionLevelAppModules_PermissionLevelId",
                table: "WorkerPermissionLevelAppModules",
                column: "PermissionLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerPermissionLevelAppModules_WorkerId",
                table: "WorkerPermissionLevelAppModules",
                column: "WorkerId");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Supports_Classes_ClassId",
            //     table: "Supports",
            //     column: "ClassId",
            //     principalTable: "Classes",
            //     principalColumn: "Id");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Supports_SupportCategories_SupportCategoryId",
            //     table: "Supports",
            //     column: "SupportCategoryId",
            //     principalTable: "SupportCategories",
            //     principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Supports_Classes_ClassId",
            //     table: "Supports");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_Supports_SupportCategories_SupportCategoryId",
            //     table: "Supports");

            migrationBuilder.DropTable(
                name: "EndpointAccesses");

            // migrationBuilder.DropTable(
            //     name: "Notifications");

            migrationBuilder.DropTable(
                name: "PermissionAccessPermissionLevel");

            migrationBuilder.DropTable(
                name: "PermissionGroupPermissionLevelAppModules");

            migrationBuilder.DropTable(
                name: "PermissionGroupWorker");

            // migrationBuilder.DropTable(
            //     name: "SupportCategories");

            migrationBuilder.DropTable(
                name: "WorkerPermissionLevelAppModules");

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

            // migrationBuilder.DropIndex(
            //     name: "IX_Supports_ClassId",
            //     table: "Supports");

            // migrationBuilder.DropIndex(
            //     name: "IX_Supports_SupportCategoryId",
            //     table: "Supports");

            migrationBuilder.DropColumn(
                name: "AvatarColor",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Fincode",
                table: "Users");

            // migrationBuilder.DropColumn(
            //     name: "ClassId",
            //     table: "Supports");

            // migrationBuilder.DropColumn(
            //     name: "Note",
            //     table: "Supports");

            // migrationBuilder.DropColumn(
            //     name: "Status",
            //     table: "Supports");

            // migrationBuilder.DropColumn(
            //     name: "SupportCategoryId",
            //     table: "Supports");

            // migrationBuilder.AddColumn<int>(
            //     name: "Id",
            //     table: "UserRoles",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0)
            //     .Annotation("SqlServer:Identity", "1, 1");

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_UserRoles",
            //     table: "UserRoles",
            //     column: "Id");

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserRoles_UserId",
            //     table: "UserRoles",
            //     column: "UserId");
        }
    }
}
