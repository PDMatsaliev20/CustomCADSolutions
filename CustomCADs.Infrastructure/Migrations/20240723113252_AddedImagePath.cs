using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Cads",
                newName: "CadPath");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Path to Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Cads");

            migrationBuilder.RenameColumn(
                name: "CadPath",
                table: "Cads",
                newName: "Path");
        }
    }
}
