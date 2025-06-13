using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWRATable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WRA_Person_PatientId",
                table: "WRA");

            migrationBuilder.DropForeignKey(
                name: "FK_WRA_Person_UserPersonId",
                table: "WRA");

            migrationBuilder.DropIndex(
                name: "IX_WRA_UserPersonId",
                table: "WRA");

            migrationBuilder.DropColumn(
                name: "UserPersonId",
                table: "WRA");

            migrationBuilder.DropColumn(
                name: "userID",
                table: "WRA");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "WRA",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_WRA_PatientId",
                table: "WRA",
                newName: "IX_WRA_PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_WRA_Person_PersonId",
                table: "WRA",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WRA_Person_PersonId",
                table: "WRA");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "WRA",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_WRA_PersonId",
                table: "WRA",
                newName: "IX_WRA_PatientId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserPersonId",
                table: "WRA",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userID",
                table: "WRA",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WRA_UserPersonId",
                table: "WRA",
                column: "UserPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_WRA_Person_PatientId",
                table: "WRA",
                column: "PatientId",
                principalTable: "Person",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WRA_Person_UserPersonId",
                table: "WRA",
                column: "UserPersonId",
                principalTable: "Person",
                principalColumn: "PersonId");
        }
    }
}
