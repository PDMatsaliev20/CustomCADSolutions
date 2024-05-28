using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MadeCadPathNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Path to 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Path to 3D Model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Path to 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Path to 3D Model");
        }
    }
}
