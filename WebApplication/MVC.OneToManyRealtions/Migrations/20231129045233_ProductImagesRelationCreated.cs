using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.OneToManyRealtions.Migrations
{
    public partial class ProductImagesRelationCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookId",
                table: "ProductImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "ProductImages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
