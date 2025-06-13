using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoCot_HC_BE.Migrations
{
    /// <inheritdoc />
    public partial class drop_table_user_department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDepartment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
