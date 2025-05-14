using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreateDentalTreatment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DentalTreatment",
                columns: table => new
                {
                    DentalTreatmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    PatientRegistryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DatePreferred = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAccepted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    acceptedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QueueNo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentalTreatment", x => x.DentalTreatmentId);
                    table.ForeignKey(
                        name: "FK_DentalTreatment_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DentalTreatment_PatientRegistry_PatientRegistryId",
                        column: x => x.PatientRegistryId,
                        principalTable: "PatientRegistry",
                        principalColumn: "PatientRegistryId");
                    table.ForeignKey(
                        name: "FK_DentalTreatment_Person_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DentalTreatment_FacilityId",
                table: "DentalTreatment",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalTreatment_PatientId",
                table: "DentalTreatment",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DentalTreatment_PatientRegistryId",
                table: "DentalTreatment",
                column: "PatientRegistryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentalTreatment");
        }
    }
}
