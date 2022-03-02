using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorGameDotNet6.Server.Migrations
{
    public partial class Coins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bananas",
                table: "Users",
                newName: "Coins");

            migrationBuilder.RenameColumn(
                name: "BananaCost",
                table: "Units",
                newName: "CoinCost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coins",
                table: "Users",
                newName: "Bananas");

            migrationBuilder.RenameColumn(
                name: "CoinCost",
                table: "Units",
                newName: "BananaCost");
        }
    }
}
