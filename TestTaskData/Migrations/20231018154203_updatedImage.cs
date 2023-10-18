using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTaskData.Migrations
{
    /// <inheritdoc />
    public partial class updatedImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");
        }
    }
}
