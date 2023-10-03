using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftVersionControl.Migrations
{
    public partial class InitFInal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Softwares");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Softwares",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
