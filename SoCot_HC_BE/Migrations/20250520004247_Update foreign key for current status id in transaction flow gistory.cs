using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Updateforeignkeyforcurrentstatusidintransactionflowgistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionFlowHistory_Status_NewStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionFlowHistory_NewStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.DropColumn(
                name: "NewStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionFlowHistory_CurrentStatusId",
                table: "TransactionFlowHistory",
                column: "CurrentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionFlowHistory_Status_CurrentStatusId",
                table: "TransactionFlowHistory",
                column: "CurrentStatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionFlowHistory_Status_CurrentStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionFlowHistory_CurrentStatusId",
                table: "TransactionFlowHistory");

            migrationBuilder.AddColumn<byte>(
                name: "NewStatusId",
                table: "TransactionFlowHistory",
                type: "tinyint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionFlowHistory_NewStatusId",
                table: "TransactionFlowHistory",
                column: "NewStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionFlowHistory_Status_NewStatusId",
                table: "TransactionFlowHistory",
                column: "NewStatusId",
                principalTable: "Status",
                principalColumn: "Id");
        }
    }
}
