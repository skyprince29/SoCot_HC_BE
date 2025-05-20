using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class RevertDentalRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentalRecordDetailsFindings");

            migrationBuilder.DropTable(
                name: "DentalRecordDetailsServices");

            migrationBuilder.DropTable(
                name: "DentalRecord");

            migrationBuilder.DropTable(
                name: "DentalRecordDetailsMedicalHistory");

            migrationBuilder.DropTable(
                name: "DentalRecordDetailsOralHealthCondition");

            migrationBuilder.DropTable(
                name: "DentalRecordDetailsPresence");

            migrationBuilder.DropTable(
                name: "DentalRecordDetailsSocialHistory");

            migrationBuilder.DropTable(
                name: "DentalRecordDetailsToothCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsMedicalHistory",
                columns: table => new
                {
                    DentalRecordDetailsMedicalHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Alergies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BloodTransfusionMonth = table.Column<int>(type: "int", nullable: true),
                    BloodTransfusionYear = table.Column<int>(type: "int", nullable: true),
                    HasAlergies = table.Column<bool>(type: "bit", nullable: false),
                    HasBloodDisorders = table.Column<bool>(type: "bit", nullable: false),
                    HasBloodTransfusion = table.Column<bool>(type: "bit", nullable: false),
                    HasCardiovascularOrHeartDiseases = table.Column<bool>(type: "bit", nullable: false),
                    HasDiabetesMelitus = table.Column<bool>(type: "bit", nullable: false),
                    HasHepatitis = table.Column<bool>(type: "bit", nullable: false),
                    HasHistoryOfPrevHospitalization = table.Column<bool>(type: "bit", nullable: false),
                    HasHypertentionOrCVA = table.Column<bool>(type: "bit", nullable: false),
                    HasMalignancy = table.Column<bool>(type: "bit", nullable: false),
                    HasOthers = table.Column<bool>(type: "bit", nullable: false),
                    HasTattoo = table.Column<bool>(type: "bit", nullable: false),
                    HasThyroidDisorders = table.Column<bool>(type: "bit", nullable: false),
                    HepatitisType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MalignancyType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Medical = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Others = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Surgical = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsMedicalHistory", x => x.DentalRecordDetailsMedicalHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsOralHealthCondition",
                columns: table => new
                {
                    DentalRecordDetailsOralHealthConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AbnormalGrowth = table.Column<bool>(type: "bit", nullable: false),
                    Calculus = table.Column<bool>(type: "bit", nullable: false),
                    CleftLipOrPalate = table.Column<bool>(type: "bit", nullable: false),
                    DateOfOralExamination = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Debris = table.Column<bool>(type: "bit", nullable: false),
                    DentalCarries = table.Column<bool>(type: "bit", nullable: false),
                    Gingivitis = table.Column<bool>(type: "bit", nullable: false),
                    NoOfDecayedTeethBigD = table.Column<int>(type: "int", nullable: false),
                    NoOfDecayedTeethSmallD = table.Column<int>(type: "int", nullable: false),
                    NoOfFilledTeethBigF = table.Column<int>(type: "int", nullable: false),
                    NoOfFilledTeethSmallF = table.Column<int>(type: "int", nullable: false),
                    NoOfMissingTeethM = table.Column<int>(type: "int", nullable: false),
                    NoPermSoundTeeth = table.Column<int>(type: "int", nullable: false),
                    NoPermTeethPresent = table.Column<int>(type: "int", nullable: false),
                    NoTempSoundTeeth = table.Column<int>(type: "int", nullable: false),
                    NoTempTeethPresent = table.Column<int>(type: "int", nullable: false),
                    OrallyFitChild = table.Column<bool>(type: "bit", nullable: false),
                    Others = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PeriodontalDisease = table.Column<bool>(type: "bit", nullable: false),
                    TotalDFTeeth = table.Column<int>(type: "int", nullable: false),
                    TotalDMFTeeth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsOralHealthCondition", x => x.DentalRecordDetailsOralHealthConditionId);
                });

            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsPresence",
                columns: table => new
                {
                    DentalRecordDetailsPresenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AgeLastBirthday = table.Column<int>(type: "int", nullable: false),
                    DateOfExamination = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PresenceOfCalculus = table.Column<bool>(type: "bit", nullable: false),
                    PresenceOfDentalCarries = table.Column<bool>(type: "bit", nullable: false),
                    PresenceOfDentoFacialAnomaly = table.Column<bool>(type: "bit", nullable: false),
                    PresenceOfGingivitis = table.Column<bool>(type: "bit", nullable: false),
                    PresenceOfNeoplasm = table.Column<bool>(type: "bit", nullable: false),
                    PresenceOfOralDebris = table.Column<bool>(type: "bit", nullable: false),
                    PresenceOfPeriodicPocket = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsPresence", x => x.DentalRecordDetailsPresenceId);
                });

            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsSocialHistory",
                columns: table => new
                {
                    DentalRecordDetailsSocialHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BetelNutChewing = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    HasBetelNutChewing = table.Column<bool>(type: "bit", nullable: false),
                    HasSweetenedSugarBeverageOrFood = table.Column<bool>(type: "bit", nullable: false),
                    HasUseOfAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    HasUseOfTobacco = table.Column<bool>(type: "bit", nullable: false),
                    SweetenedSugarBeverageOrFood = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UseOfAlcohol = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UseOfTobacco = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsSocialHistory", x => x.DentalRecordDetailsSocialHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsToothCount",
                columns: table => new
                {
                    DentalRecordDetailsToothCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarriesIndicatedForExtractionPerm = table.Column<int>(type: "int", nullable: false),
                    CarriesIndicatedForExtractionTemp = table.Column<int>(type: "int", nullable: false),
                    CarriesIndicatedForFillingPerm = table.Column<int>(type: "int", nullable: false),
                    CarriesIndicatedForFillingTemp = table.Column<int>(type: "int", nullable: false),
                    Examiner = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FilledOrRestoredPerm = table.Column<int>(type: "int", nullable: false),
                    FilledOrRestoredTemp = table.Column<int>(type: "int", nullable: false),
                    FluorideApplication = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MissingDueToCarries = table.Column<int>(type: "int", nullable: false),
                    NoOfTeethPresentPerm = table.Column<int>(type: "int", nullable: false),
                    NoOfTeethPresentTemp = table.Column<int>(type: "int", nullable: false),
                    RootFragmentPerm = table.Column<int>(type: "int", nullable: false),
                    RootFragmentTemp = table.Column<int>(type: "int", nullable: false),
                    TotalDfAndDmfTeeth = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsToothCount", x => x.DentalRecordDetailsToothCountId);
                });

            migrationBuilder.CreateTable(
                name: "DentalRecord",
                columns: table => new
                {
                    DentalRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DentalRecordDetailsMedicalHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DentalRecordDetailsOralHealthConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DentalRecordDetailsPresenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DentalRecordDetailsSocialHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DentalRecordDetailsToothCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PhysicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConsentedByName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRecord = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DentalRecordDetailsPersenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReferralId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferralNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecord", x => x.DentalRecordId);
                    table.ForeignKey(
                        name: "FK_DentalRecord_DentalRecordDetailsMedicalHistory_DentalRecordDetailsMedicalHistoryId",
                        column: x => x.DentalRecordDetailsMedicalHistoryId,
                        principalTable: "DentalRecordDetailsMedicalHistory",
                        principalColumn: "DentalRecordDetailsMedicalHistoryId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_DentalRecordDetailsOralHealthCondition_DentalRecordDetailsOralHealthConditionId",
                        column: x => x.DentalRecordDetailsOralHealthConditionId,
                        principalTable: "DentalRecordDetailsOralHealthCondition",
                        principalColumn: "DentalRecordDetailsOralHealthConditionId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_DentalRecordDetailsPresence_DentalRecordDetailsPresenceId",
                        column: x => x.DentalRecordDetailsPresenceId,
                        principalTable: "DentalRecordDetailsPresence",
                        principalColumn: "DentalRecordDetailsPresenceId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_DentalRecordDetailsSocialHistory_DentalRecordDetailsSocialHistoryId",
                        column: x => x.DentalRecordDetailsSocialHistoryId,
                        principalTable: "DentalRecordDetailsSocialHistory",
                        principalColumn: "DentalRecordDetailsSocialHistoryId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_DentalRecordDetailsToothCount_DentalRecordDetailsToothCountId",
                        column: x => x.DentalRecordDetailsToothCountId,
                        principalTable: "DentalRecordDetailsToothCount",
                        principalColumn: "DentalRecordDetailsToothCountId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_Person_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_DentalRecord_Person_PhysicianId",
                        column: x => x.PhysicianId,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsFindings",
                columns: table => new
                {
                    DentalRecordDetailsFindingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DateDiagnose = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DentalRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ToothNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsFindings", x => x.DentalRecordDetailsFindingsId);
                    table.ForeignKey(
                        name: "FK_DentalRecordDetailsFindings_DentalRecord_DentalRecordId",
                        column: x => x.DentalRecordId,
                        principalTable: "DentalRecord",
                        principalColumn: "DentalRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DentalRecordDetailsServices",
                columns: table => new
                {
                    DentalRecordDetailsServicesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateDiagnose = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DentalRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ServiceRendered = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ToothNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalRecordDetailsServices", x => x.DentalRecordDetailsServicesId);
                    table.ForeignKey(
                        name: "FK_DentalRecordDetailsServices_DentalRecord_DentalRecordId",
                        column: x => x.DentalRecordId,
                        principalTable: "DentalRecord",
                        principalColumn: "DentalRecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_DentalRecordDetailsMedicalHistoryId",
                table: "DentalRecord",
                column: "DentalRecordDetailsMedicalHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_DentalRecordDetailsOralHealthConditionId",
                table: "DentalRecord",
                column: "DentalRecordDetailsOralHealthConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_DentalRecordDetailsPresenceId",
                table: "DentalRecord",
                column: "DentalRecordDetailsPresenceId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_DentalRecordDetailsSocialHistoryId",
                table: "DentalRecord",
                column: "DentalRecordDetailsSocialHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_DentalRecordDetailsToothCountId",
                table: "DentalRecord",
                column: "DentalRecordDetailsToothCountId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_FacilityId",
                table: "DentalRecord",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_PatientId",
                table: "DentalRecord",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecord_PhysicianId",
                table: "DentalRecord",
                column: "PhysicianId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecordDetailsFindings_DentalRecordId",
                table: "DentalRecordDetailsFindings",
                column: "DentalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalRecordDetailsServices_DentalRecordId",
                table: "DentalRecordDetailsServices",
                column: "DentalRecordId");
        }
    }
}
