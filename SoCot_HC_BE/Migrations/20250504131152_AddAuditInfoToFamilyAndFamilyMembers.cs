using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditInfoToFamilyAndFamilyMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
               name: "CreatedBy",
               table: "FamilyMembers",
               type: "uniqueidentifier",
               nullable: false,
               defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "FamilyMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "FamilyMembers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "FamilyMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Families",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Families",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Families",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Families",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Families");
        }
    }
}
