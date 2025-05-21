using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Updatepatientregistryaddstatuscolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "StatusId",
                table: "PatientRegistry",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientRegistry_StatusId",
                table: "PatientRegistry",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientRegistry_Status_StatusId",
                table: "PatientRegistry",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientRegistry_Status_StatusId",
                table: "PatientRegistry");

            migrationBuilder.DropIndex(
                name: "IX_PatientRegistry_StatusId",
                table: "PatientRegistry");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PatientRegistry");
        }
    }
}
