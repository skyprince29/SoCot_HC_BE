using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class updateItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_UoM_UoMId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_UoMId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "UoMId",
                table: "Item");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UoMId",
                table: "Item",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Item_UoMId",
                table: "Item",
                column: "UoMId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_UoM_UoMId",
                table: "Item",
                column: "UoMId",
                principalTable: "UoM",
                principalColumn: "UoMId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
