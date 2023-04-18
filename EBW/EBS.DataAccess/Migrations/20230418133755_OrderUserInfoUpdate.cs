using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electronic_Bookstore_Web.Migrations
{
    public partial class OrderUserInfoUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentIntendId",
                table: "OrderUserInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "OrderUserInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderUserInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntendId",
                table: "OrderUserInfos");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderUserInfos");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderUserInfos");
        }
    }
}
