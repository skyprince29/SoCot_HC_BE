using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    UserAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    RememberMeToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsinitLogin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserIdTemp = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.UserAccountId);
                    table.ForeignKey(
                        name: "FK_UserAccount_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccount_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAccount_UserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroup",
                        principalColumn: "UserGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_FacilityId",
                table: "UserAccount",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_PersonId",
                table: "UserAccount",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_UserGroupId",
                table: "UserAccount",
                column: "UserGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccount");
        }
    }
}
