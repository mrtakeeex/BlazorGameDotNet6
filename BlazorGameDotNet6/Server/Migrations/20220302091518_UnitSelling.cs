using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorGameDotNet6.Server.Migrations
{
    public partial class UnitSelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentValue",
                table: "UserUnits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentValue",
                table: "UserUnits");
        }
    }
}
