using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatient_AddPersonRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.CreateTable(
                name: "PersonRelation",
                columns: table => new
                {
                    PersonRelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonIdSelf = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonIdRelated = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRelation", x => x.PersonRelationId);
                    table.ForeignKey(
                        name: "FK_PersonRelation_Person_PersonIdRelated",
                        column: x => x.PersonIdRelated,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonRelation_Person_PersonIdSelf",
                        column: x => x.PersonIdSelf,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonRelation_PersonIdRelated",
                table: "PersonRelation",
                column: "PersonIdRelated");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRelation_PersonIdSelf",
                table: "PersonRelation",
                column: "PersonIdSelf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonRelation");

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonIdFather = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PersonIdMother = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PersonIdPatient = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonIdSpouse = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PHICMemberType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PatientIdTemp = table.Column<int>(type: "int", nullable: false),
                    PhilHealthNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patient_Person_PersonIdFather",
                        column: x => x.PersonIdFather,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_Patient_Person_PersonIdMother",
                        column: x => x.PersonIdMother,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_Patient_Person_PersonIdPatient",
                        column: x => x.PersonIdPatient,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Person_PersonIdSpouse",
                        column: x => x.PersonIdSpouse,
                        principalTable: "Person",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PersonIdFather",
                table: "Patient",
                column: "PersonIdFather");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PersonIdMother",
                table: "Patient",
                column: "PersonIdMother");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PersonIdPatient",
                table: "Patient",
                column: "PersonIdPatient");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_PersonIdSpouse",
                table: "Patient",
                column: "PersonIdSpouse");
        }
    }
}
