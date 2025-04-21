using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Create_Household : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Household",
                columns: table => new
                {
                    HouseholdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseholdNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ResidenceName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PersonIdHeadOfHousehold = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Household", x => x.HouseholdId);
                    table.ForeignKey(
                        name: "FK_Household_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Household_Person_PersonIdHeadOfHousehold",
                        column: x => x.PersonIdHeadOfHousehold,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Household_AddressId",
                table: "Household",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Household_PersonIdHeadOfHousehold",
                table: "Household",
                column: "PersonIdHeadOfHousehold");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Household");
        }
    }
}
