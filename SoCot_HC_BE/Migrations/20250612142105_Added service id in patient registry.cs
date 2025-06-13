using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Addedserviceidinpatientregistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "PatientRegistry",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientRegistry_ServiceId",
                table: "PatientRegistry",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientRegistry_Service_ServiceId",
                table: "PatientRegistry",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientRegistry_Service_ServiceId",
                table: "PatientRegistry");

            migrationBuilder.DropIndex(
                name: "IX_PatientRegistry_ServiceId",
                table: "PatientRegistry");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "PatientRegistry");
        }
    }
}
