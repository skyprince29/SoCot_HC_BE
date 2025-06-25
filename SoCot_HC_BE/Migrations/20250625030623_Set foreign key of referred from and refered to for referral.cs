using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Setforeignkeyofreferredfromandreferedtoforreferral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Referral_Facility_FacilityReferredFromFacilityId",
                table: "Referral");

            migrationBuilder.DropForeignKey(
                name: "FK_Referral_Facility_FacilityReferredToFacilityId",
                table: "Referral");

            migrationBuilder.DropIndex(
                name: "IX_Referral_FacilityReferredFromFacilityId",
                table: "Referral");

            migrationBuilder.DropIndex(
                name: "IX_Referral_FacilityReferredToFacilityId",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "FacilityReferredFromFacilityId",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "FacilityReferredToFacilityId",
                table: "Referral");

            migrationBuilder.CreateIndex(
                name: "IX_Referral_ReferredFrom",
                table: "Referral",
                column: "ReferredFrom");

            migrationBuilder.CreateIndex(
                name: "IX_Referral_ReferredTo",
                table: "Referral",
                column: "ReferredTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Referral_Facility_ReferredFrom",
                table: "Referral",
                column: "ReferredFrom",
                principalTable: "Facility",
                principalColumn: "FacilityId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Referral_Facility_ReferredTo",
                table: "Referral",
                column: "ReferredTo",
                principalTable: "Facility",
                principalColumn: "FacilityId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Referral_Facility_ReferredFrom",
                table: "Referral");

            migrationBuilder.DropForeignKey(
                name: "FK_Referral_Facility_ReferredTo",
                table: "Referral");

            migrationBuilder.DropIndex(
                name: "IX_Referral_ReferredFrom",
                table: "Referral");

            migrationBuilder.DropIndex(
                name: "IX_Referral_ReferredTo",
                table: "Referral");

            migrationBuilder.AddColumn<int>(
                name: "FacilityReferredFromFacilityId",
                table: "Referral",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilityReferredToFacilityId",
                table: "Referral",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referral_FacilityReferredFromFacilityId",
                table: "Referral",
                column: "FacilityReferredFromFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Referral_FacilityReferredToFacilityId",
                table: "Referral",
                column: "FacilityReferredToFacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referral_Facility_FacilityReferredFromFacilityId",
                table: "Referral",
                column: "FacilityReferredFromFacilityId",
                principalTable: "Facility",
                principalColumn: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referral_Facility_FacilityReferredToFacilityId",
                table: "Referral",
                column: "FacilityReferredToFacilityId",
                principalTable: "Facility",
                principalColumn: "FacilityId");
        }
    }
}
