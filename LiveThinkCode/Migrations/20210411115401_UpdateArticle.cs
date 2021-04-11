using Microsoft.EntityFrameworkCore.Migrations;

namespace LiveThinkCode.Migrations
{
    public partial class UpdateArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LinkSlug",
                table: "Articles",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Articles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Articles",
                newName: "LinkSlug");
        }
    }
}
