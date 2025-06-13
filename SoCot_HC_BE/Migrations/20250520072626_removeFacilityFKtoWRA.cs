using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class removeFacilityFKtoWRA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WRA_Facility_FacilityId",
                table: "WRA");

            migrationBuilder.DropIndex(
                name: "IX_WRA_FacilityId",
                table: "WRA");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "WRA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "WRA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WRA_FacilityId",
                table: "WRA",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WRA_Facility_FacilityId",
                table: "WRA",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "FacilityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
