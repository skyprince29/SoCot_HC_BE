using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreatetablePatientDepartmentTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientDepartmentTransaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientRegistryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForwardedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AcceptedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDepartmentTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDepartmentTransaction_PatientRegistry_PatientRegistryId",
                        column: x => x.PatientRegistryId,
                        principalTable: "PatientRegistry",
                        principalColumn: "PatientRegistryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDepartmentTransaction_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientDepartmentTransaction_PatientRegistryId",
                table: "PatientDepartmentTransaction",
                column: "PatientRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDepartmentTransaction_StatusId",
                table: "PatientDepartmentTransaction",
                column: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientDepartmentTransaction");
        }
    }
}
