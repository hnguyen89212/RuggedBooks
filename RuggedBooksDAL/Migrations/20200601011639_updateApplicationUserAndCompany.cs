using Microsoft.EntityFrameworkCore.Migrations;

namespace RuggedBooksDAL.Migrations
{
    public partial class updateApplicationUserAndCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Companies",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Companies");
        }
    }
}
