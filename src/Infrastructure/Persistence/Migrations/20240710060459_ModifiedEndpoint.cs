using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Space.Infrastructure.Persistence.Migrations
{
    public partial class ModifiedEndpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointDetails");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndpointAccesses");

            migrationBuilder.CreateTable(
                name: "EndpointDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ApplicationModuleId = table.Column<int>(type: "int", nullable: true),
                    PermissionAccessId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndpointDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndpointDetails_ApplicationModules_ApplicationModuleId",
                        column: x => x.ApplicationModuleId,
                        principalTable: "ApplicationModules",
                        principalColumn: "Id");
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
                name: "IX_EndpointDetails_ApplicationModuleId",
                table: "EndpointDetails",
                column: "ApplicationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EndpointDetails_PermissionAccessId",
                table: "EndpointDetails",
                column: "PermissionAccessId");
        }
    }
}
