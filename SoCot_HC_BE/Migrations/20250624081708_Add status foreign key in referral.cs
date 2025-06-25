using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Addstatusforeignkeyinreferral : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Referral_StatusId",
                table: "Referral",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Referral_Status_StatusId",
                table: "Referral",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Referral_Status_StatusId",
                table: "Referral");

            migrationBuilder.DropIndex(
                name: "IX_Referral_StatusId",
                table: "Referral");
        }
    }
}
