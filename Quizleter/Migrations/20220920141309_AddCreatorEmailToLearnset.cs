using Microsoft.EntityFrameworkCore.Migrations;

namespace Quizleter.Migrations
{
    public partial class AddCreatorEmailToLearnset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorEmail",
                table: "Learnset",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorEmail",
                table: "Learnset");
        }
    }
}
