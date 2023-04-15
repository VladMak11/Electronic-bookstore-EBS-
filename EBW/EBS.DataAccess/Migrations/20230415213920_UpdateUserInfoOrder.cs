using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electronic_Bookstore_Web.Migrations
{
    public partial class UpdateUserInfoOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "OrderUserInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "OrderUserInfos");
        }
    }
}
