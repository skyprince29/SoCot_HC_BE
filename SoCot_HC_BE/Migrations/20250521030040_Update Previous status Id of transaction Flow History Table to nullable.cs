using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePreviousstatusIdoftransactionFlowHistoryTabletonullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionFlowHistory_Status_PreviousStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.AlterColumn<byte>(
                name: "PreviousStatusId",
                table: "TransactionFlowHistory",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionFlowHistory_Status_PreviousStatusId",
                table: "TransactionFlowHistory",
                column: "PreviousStatusId",
                principalTable: "Status",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionFlowHistory_Status_PreviousStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.AlterColumn<byte>(
                name: "PreviousStatusId",
                table: "TransactionFlowHistory",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionFlowHistory_Status_PreviousStatusId",
                table: "TransactionFlowHistory",
                column: "PreviousStatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
