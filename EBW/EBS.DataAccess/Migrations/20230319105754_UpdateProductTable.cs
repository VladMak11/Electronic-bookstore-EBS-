using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electronic_Bookstore_Web.Migrations
{
    public partial class UpdateProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISmageUrl",
                table: "Products",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ISmageUrl");
        }
    }
}
