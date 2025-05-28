using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class createtablenoncommunicabledisease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FamilyHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hypertension = table.Column<bool>(type: "bit", nullable: false),
                    Stroke = table.Column<bool>(type: "bit", nullable: false),
                    HeartAttack = table.Column<bool>(type: "bit", nullable: false),
                    Diabetes = table.Column<bool>(type: "bit", nullable: false),
                    Asthma = table.Column<bool>(type: "bit", nullable: false),
                    Cancer = table.Column<bool>(type: "bit", nullable: false),
                    KidneyDisease = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NonCommunicableDisease",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FamilyHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Waist = table.Column<int>(type: "int", nullable: false),
                    FirstVitalSignId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondVitalSignId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AverageBP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Smoking = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlcoholIntake = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExcessiveAlcoholIntake = table.Column<bool>(type: "bit", nullable: false),
                    HighFatSalt = table.Column<bool>(type: "bit", nullable: false),
                    Vegetable = table.Column<bool>(type: "bit", nullable: false),
                    Fruits = table.Column<bool>(type: "bit", nullable: false),
                    PhysicalActivity = table.Column<bool>(type: "bit", nullable: false),
                    withDiabetes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Polyphagia = table.Column<bool>(type: "bit", nullable: false),
                    Polydipsia = table.Column<bool>(type: "bit", nullable: false),
                    Polyuria = table.Column<bool>(type: "bit", nullable: false),
                    withKetones = table.Column<bool>(type: "bit", nullable: false),
                    Ketones = table.Column<int>(type: "int", nullable: false),
                    KetonesDateTaken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    withProtein = table.Column<bool>(type: "bit", nullable: false),
                    UrineProtein = table.Column<int>(type: "int", nullable: false),
                    UrineProteinDateTaken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Glucose = table.Column<bool>(type: "bit", nullable: false),
                    FBS_RBS = table.Column<int>(type: "int", nullable: false),
                    GlucoseDateTaken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lipids = table.Column<bool>(type: "bit", nullable: false),
                    TotalCholesterol = table.Column<int>(type: "int", nullable: false),
                    LipidsDateTaken = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnginaHeart = table.Column<bool>(type: "bit", nullable: false),
                    hasStrokeTIA = table.Column<bool>(type: "bit", nullable: false),
                    riskLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCDQ8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAssed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonCommunicableDisease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonCommunicableDisease_FamilyHistory_FamilyHistoryId",
                        column: x => x.FamilyHistoryId,
                        principalTable: "FamilyHistory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NonCommunicableDisease_Person_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_NonCommunicableDisease_VitalSigns_FirstVitalSignId",
                        column: x => x.FirstVitalSignId,
                        principalTable: "VitalSigns",
                        principalColumn: "VitalSignId");
                    table.ForeignKey(
                        name: "FK_NonCommunicableDisease_VitalSigns_SecondVitalSignId",
                        column: x => x.SecondVitalSignId,
                        principalTable: "VitalSigns",
                        principalColumn: "VitalSignId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NonCommunicableDisease_FamilyHistoryId",
                table: "NonCommunicableDisease",
                column: "FamilyHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NonCommunicableDisease_FirstVitalSignId",
                table: "NonCommunicableDisease",
                column: "FirstVitalSignId");

            migrationBuilder.CreateIndex(
                name: "IX_NonCommunicableDisease_PatientId",
                table: "NonCommunicableDisease",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NonCommunicableDisease_SecondVitalSignId",
                table: "NonCommunicableDisease",
                column: "SecondVitalSignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NonCommunicableDisease");

            migrationBuilder.DropTable(
                name: "FamilyHistory");
        }
    }
}
