using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Addpersonidinreferral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "Referral",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referral_PersonId",
                table: "Referral",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referral_Person_PersonId",
                table: "Referral",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Referral_Person_PersonId",
                table: "Referral");

            migrationBuilder.DropIndex(
                name: "IX_Referral_PersonId",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Referral");
        }
    }
}
