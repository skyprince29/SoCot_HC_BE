using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreateTransactionflowhistorytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionFlowHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    PreviousStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    CurrentStatusId = table.Column<byte>(type: "tinyint", nullable: false),
                    NewStatusId = table.Column<byte>(type: "tinyint", nullable: true),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionFlowHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionFlowHistory_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionFlowHistory_Status_NewStatusId",
                        column: x => x.NewStatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionFlowHistory_Status_PreviousStatusId",
                        column: x => x.PreviousStatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionFlowHistory_ModuleId",
                table: "TransactionFlowHistory",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionFlowHistory_NewStatusId",
                table: "TransactionFlowHistory",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionFlowHistory_PreviousStatusId",
                table: "TransactionFlowHistory",
                column: "PreviousStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionFlowHistory");
        }
    }
}
