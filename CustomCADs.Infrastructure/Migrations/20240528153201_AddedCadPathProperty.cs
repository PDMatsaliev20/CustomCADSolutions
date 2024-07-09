using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCadPathProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Extension name of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Path to 3D Model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Cads");

            migrationBuilder.AlterColumn<string>(
                name: "Extension",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Extension name of 3D Model");
        }
    }
}
