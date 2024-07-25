using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Cads");

            migrationBuilder.AddColumn<string>(
                name: "CadExtension",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Extension of Image file");

            migrationBuilder.AddColumn<string>(
                name: "ImageExtension",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Extension of 3D Model file");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadExtension",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "ImageExtension",
                table: "Cads");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Extension name of 3D Model");
        }
    }
}
