using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Addeddesignationinuseraccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesignationId",
                table: "UserAccount",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_DesignationId",
                table: "UserAccount",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_Designation_DesignationId",
                table: "UserAccount",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "DesignationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_Designation_DesignationId",
                table: "UserAccount");

            migrationBuilder.DropIndex(
                name: "IX_UserAccount_DesignationId",
                table: "UserAccount");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "UserAccount");
        }
    }
}
