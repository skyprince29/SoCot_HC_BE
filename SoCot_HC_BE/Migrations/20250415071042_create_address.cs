using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class create_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BarangayId = table.Column<int>(type: "int", nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false),
                    Sitio = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Purok = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    HouseNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LotNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BlockNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Subdivision = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_Barangay_BarangayId",
                        column: x => x.BarangayId,
                        principalTable: "Barangay",
                        principalColumn: "BarangayId",
                        onDelete: ReferentialAction.Cascade);  // Keep Cascade for Barangay
                    table.ForeignKey(
                        name: "FK_Address_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipality",
                        principalColumn: "MunicipalityId",
                        onDelete: ReferentialAction.Restrict);  // Change to Restrict for Municipality
                    table.ForeignKey(
                        name: "FK_Address_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);  // Change to Restrict for Province
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_BarangayId",
                table: "Address",
                column: "BarangayId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_MunicipalityId",
                table: "Address",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_ProvinceId",
                table: "Address",
                column: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
