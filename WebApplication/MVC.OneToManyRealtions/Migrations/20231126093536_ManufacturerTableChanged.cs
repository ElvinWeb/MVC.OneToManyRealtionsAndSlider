using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.OneToManyRealtions.Migrations
{
    public partial class ManufacturerTableChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manufacturers_Manufacturers_ManufacturerId",
                table: "Manufacturers");

            migrationBuilder.DropIndex(
                name: "IX_Manufacturers_ManufacturerId",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "Manufacturers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "Manufacturers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_ManufacturerId",
                table: "Manufacturers",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manufacturers_Manufacturers_ManufacturerId",
                table: "Manufacturers",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id");
        }
    }
}
