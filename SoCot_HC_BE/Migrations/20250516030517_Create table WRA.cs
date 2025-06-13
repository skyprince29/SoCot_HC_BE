using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreatetableWRA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WRA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WraDateOfAssessment = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fecundity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HavePartner = table.Column<bool>(type: "bit", nullable: false),
                    WraPlanToHaveMoreChildren = table.Column<bool>(type: "bit", nullable: false),
                    WraPlanToHveMoreChildrenDecision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForCounseling = table.Column<bool>(type: "bit", nullable: false),
                    UsingAnyFPMethod = table.Column<bool>(type: "bit", nullable: false),
                    FPType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FPMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShiftToModernMethod = table.Column<bool>(type: "bit", nullable: false),
                    WraDateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: true),
                    UserPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WRA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WRA_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WRA_Person_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WRA_Person_UserPersonId",
                        column: x => x.UserPersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WRA_FacilityId",
                table: "WRA",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_WRA_PatientId",
                table: "WRA",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_WRA_UserPersonId",
                table: "WRA",
                column: "UserPersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WRA");
        }
    }
}
