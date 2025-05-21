using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Deletepatientregistrylogtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientRegistryLog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientRegistryLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientRegistryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRegistryLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientRegistryLog_PatientRegistry_PatientRegistryId",
                        column: x => x.PatientRegistryId,
                        principalTable: "PatientRegistry",
                        principalColumn: "PatientRegistryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientRegistryLog_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientRegistryLog_PatientRegistryId",
                table: "PatientRegistryLog",
                column: "PatientRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRegistryLog_StatusId",
                table: "PatientRegistryLog",
                column: "StatusId");
        }
    }
}
