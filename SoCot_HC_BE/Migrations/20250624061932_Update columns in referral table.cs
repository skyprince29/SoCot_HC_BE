using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Updatecolumnsinreferraltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionDateTime",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "ArrivalDateTime",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "DischargeDateTime",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "ReferrenceId",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Referral");

            migrationBuilder.DropColumn(
                name: "isAlreadyUse",
                table: "Referral");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Referral",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Complains",
                table: "Referral",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StatusId",
                table: "Referral",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Referral");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Referral",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Complains",
                table: "Referral",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdmissionDateTime",
                table: "Referral",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDateTime",
                table: "Referral",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DischargeDateTime",
                table: "Referral",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Referral",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ReferrenceId",
                table: "Referral",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Referral",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isAlreadyUse",
                table: "Referral",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
