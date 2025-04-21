using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Createdreferraltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Referral",
                columns: table => new
                {
                    ReferralId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempRefId = table.Column<int>(type: "int", nullable: false),
                    Complains = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferredTo = table.Column<int>(type: "int", nullable: false),
                    FacilityReferredToFacilityId = table.Column<int>(type: "int", nullable: true),
                    ReferredFrom = table.Column<int>(type: "int", nullable: false),
                    FacilityReferredFromFacilityId = table.Column<int>(type: "int", nullable: true),
                    ReferralNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferralDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdmissionDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DischargeDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DischargeInstructions = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PersonnelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttendingPhysicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    isAlreadyUse = table.Column<bool>(type: "bit", nullable: false),
                    ReferrenceId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referral", x => x.ReferralId);
                    table.ForeignKey(
                        name: "FK_Referral_Facility_FacilityReferredFromFacilityId",
                        column: x => x.FacilityReferredFromFacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId");
                    table.ForeignKey(
                        name: "FK_Referral_Facility_FacilityReferredToFacilityId",
                        column: x => x.FacilityReferredToFacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId");
                    table.ForeignKey(
                        name: "FK_Referral_Personnel_AttendingPhysicianId",
                        column: x => x.AttendingPhysicianId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                    table.ForeignKey(
                        name: "FK_Referral_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referral_AttendingPhysicianId",
                table: "Referral",
                column: "AttendingPhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Referral_FacilityReferredFromFacilityId",
                table: "Referral",
                column: "FacilityReferredFromFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Referral_FacilityReferredToFacilityId",
                table: "Referral",
                column: "FacilityReferredToFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Referral_PersonnelId",
                table: "Referral",
                column: "PersonnelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Referral");
        }
    }
}
