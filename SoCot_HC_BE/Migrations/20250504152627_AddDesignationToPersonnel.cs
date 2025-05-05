using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignationToPersonnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesignationId",
                table: "Personnel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_DesignationId",
                table: "Personnel",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnel_Designation_DesignationId",
                table: "Personnel",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "DesignationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnel_Designation_DesignationId",
                table: "Personnel");

            migrationBuilder.DropIndex(
                name: "IX_Personnel_DesignationId",
                table: "Personnel");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Personnel");
        }
    }
}
