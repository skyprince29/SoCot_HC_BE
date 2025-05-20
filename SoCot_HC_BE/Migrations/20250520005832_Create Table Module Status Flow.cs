using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableModuleStatusFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleStatusFlow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    RequiredStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    NextStatusId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleStatusFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleStatusFlow_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleStatusFlow_Status_NextStatusId",
                        column: x => x.NextStatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleStatusFlow_Status_RequiredStatusId",
                        column: x => x.RequiredStatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleStatusFlow_ModuleId",
                table: "ModuleStatusFlow",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleStatusFlow_NextStatusId",
                table: "ModuleStatusFlow",
                column: "NextStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleStatusFlow_RequiredStatusId",
                table: "ModuleStatusFlow",
                column: "RequiredStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleStatusFlow");
        }
    }
}
