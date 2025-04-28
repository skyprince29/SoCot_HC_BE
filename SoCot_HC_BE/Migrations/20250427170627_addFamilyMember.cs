using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class addFamilyMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Family_Household_HouseholdId",
                table: "Family");

            migrationBuilder.DropForeignKey(
                name: "FK_Family_Person_PersonId",
                table: "Family");

            migrationBuilder.DropForeignKey(
                name: "FK_Household_Address_AddressId",
                table: "Household");

            migrationBuilder.DropForeignKey(
                name: "FK_Household_Person_PersonIdHeadOfHousehold",
                table: "Household");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Household",
                table: "Household");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Family",
                table: "Family");

            migrationBuilder.RenameTable(
                name: "Household",
                newName: "Households");

            migrationBuilder.RenameTable(
                name: "Family",
                newName: "Families");

            migrationBuilder.RenameIndex(
                name: "IX_Household_PersonIdHeadOfHousehold",
                table: "Households",
                newName: "IX_Households_PersonIdHeadOfHousehold");

            migrationBuilder.RenameIndex(
                name: "IX_Household_AddressId",
                table: "Households",
                newName: "IX_Households_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Family_PersonId",
                table: "Families",
                newName: "IX_Families_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Family_HouseholdId",
                table: "Families",
                newName: "IX_Families_HouseholdId");

            migrationBuilder.AlterColumn<int>(
                name: "Suffix",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Religion",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<int>(
                name: "CivilStatus",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BloodType",
                table: "Person",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Households",
                table: "Households",
                column: "HouseholdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Families",
                table: "Families",
                column: "FamilyId");

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    FamilyMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.FamilyMemberId);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_PersonId",
                table: "FamilyMembers",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Households_HouseholdId",
                table: "Families",
                column: "HouseholdId",
                principalTable: "Households",
                principalColumn: "HouseholdId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_Person_PersonId",
                table: "Families",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Households_Address_AddressId",
                table: "Households",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Households_Person_PersonIdHeadOfHousehold",
                table: "Households",
                column: "PersonIdHeadOfHousehold",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Families_Households_HouseholdId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_Person_PersonId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_Households_Address_AddressId",
                table: "Households");

            migrationBuilder.DropForeignKey(
                name: "FK_Households_Person_PersonIdHeadOfHousehold",
                table: "Households");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Households",
                table: "Households");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Families",
                table: "Families");

            migrationBuilder.RenameTable(
                name: "Households",
                newName: "Household");

            migrationBuilder.RenameTable(
                name: "Families",
                newName: "Family");

            migrationBuilder.RenameIndex(
                name: "IX_Households_PersonIdHeadOfHousehold",
                table: "Household",
                newName: "IX_Household_PersonIdHeadOfHousehold");

            migrationBuilder.RenameIndex(
                name: "IX_Households_AddressId",
                table: "Household",
                newName: "IX_Household_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Families_PersonId",
                table: "Family",
                newName: "IX_Family_PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Families_HouseholdId",
                table: "Family",
                newName: "IX_Family_HouseholdId");

            migrationBuilder.AlterColumn<string>(
                name: "Suffix",
                table: "Person",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Religion",
                table: "Person",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Person",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CivilStatus",
                table: "Person",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BloodType",
                table: "Person",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Household",
                table: "Household",
                column: "HouseholdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Family",
                table: "Family",
                column: "FamilyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Family_Household_HouseholdId",
                table: "Family",
                column: "HouseholdId",
                principalTable: "Household",
                principalColumn: "HouseholdId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Family_Person_PersonId",
                table: "Family",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Household_Address_AddressId",
                table: "Household",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Household_Person_PersonIdHeadOfHousehold",
                table: "Household",
                column: "PersonIdHeadOfHousehold",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
