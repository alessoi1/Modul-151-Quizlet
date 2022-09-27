using Microsoft.EntityFrameworkCore.Migrations;

namespace Quizleter.Migrations
{
    public partial class AddUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorEmail",
                table: "Learnset",
                newName: "CreatorUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorUsername",
                table: "Learnset",
                newName: "CreatorEmail");
        }
    }
}
