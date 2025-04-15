using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Addfacilityidinpatientregistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "PatientRegistry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PatientRegistry_FacilityId",
                table: "PatientRegistry",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientRegistry_Facility_FacilityId",
                table: "PatientRegistry",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "FacilityId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientRegistry_Facility_FacilityId",
                table: "PatientRegistry");

            migrationBuilder.DropIndex(
                name: "IX_PatientRegistry_FacilityId",
                table: "PatientRegistry");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "PatientRegistry");
        }
    }
}
