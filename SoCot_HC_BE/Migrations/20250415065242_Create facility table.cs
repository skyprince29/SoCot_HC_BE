using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class Createfacilitytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    FacilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccreditationNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FacilityName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TINNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FacilityLevel = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.FacilityId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Facility");
        }
    }
}
