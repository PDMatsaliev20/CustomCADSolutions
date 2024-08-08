using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCadManyProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadExtension",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "ImageExtension",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "IsFolder",
                table: "Cads");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Relative Path to Image",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Path to Image");

            migrationBuilder.AlterColumn<string>(
                name: "CadPath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Relative Path to 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Path to 3D Model");

            migrationBuilder.AddColumn<string>(
                name: "OtherFilesPaths",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherFilesPaths",
                table: "Cads");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Path to Image",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Relative Path to Image");

            migrationBuilder.AlterColumn<string>(
                name: "CadPath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Path to 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Relative Path to 3D Model");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsFolder",
                table: "Cads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
