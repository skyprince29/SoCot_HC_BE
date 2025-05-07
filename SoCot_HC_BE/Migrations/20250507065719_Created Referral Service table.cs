using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreatedReferralServicetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TempRefId",
                table: "Referral",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ReferralService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferralId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferralService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferralService_Referral_ReferralId",
                        column: x => x.ReferralId,
                        principalTable: "Referral",
                        principalColumn: "ReferralId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferralService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReferralService_ReferralId",
                table: "ReferralService",
                column: "ReferralId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralService_ServiceId",
                table: "ReferralService",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReferralService");

            migrationBuilder.AlterColumn<int>(
                name: "TempRefId",
                table: "Referral",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
