using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdatecolumnsinModuleStatusFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleStatusFlow_Status_RequiredStatusId",
                table: "ModuleStatusFlow");

            migrationBuilder.AlterColumn<byte>(
                name: "RequiredStatusId",
                table: "ModuleStatusFlow",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "ModuleStatusFlow",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStart",
                table: "ModuleStatusFlow",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleStatusFlow_Status_RequiredStatusId",
                table: "ModuleStatusFlow",
                column: "RequiredStatusId",
                principalTable: "Status",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleStatusFlow_Status_RequiredStatusId",
                table: "ModuleStatusFlow");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "ModuleStatusFlow");

            migrationBuilder.DropColumn(
                name: "IsStart",
                table: "ModuleStatusFlow");

            migrationBuilder.AlterColumn<byte>(
                name: "RequiredStatusId",
                table: "ModuleStatusFlow",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleStatusFlow_Status_RequiredStatusId",
                table: "ModuleStatusFlow",
                column: "RequiredStatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
