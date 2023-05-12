using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Get_Together_Riders.Migrations
{
    public partial class addRiderIdentityID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Riders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Riders");
        }
    }
}
