using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Createdvitalsignreferencetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientRegistryId",
                table: "VitalSigns");

            migrationBuilder.CreateTable(
                name: "VitalSignReference",
                columns: table => new
                {
                    VitalSignReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VitalSignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VitalSignReferenceType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VitalSignReference", x => x.VitalSignReferenceId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VitalSignReference");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientRegistryId",
                table: "VitalSigns",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
