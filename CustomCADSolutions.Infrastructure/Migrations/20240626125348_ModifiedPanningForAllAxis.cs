using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedPanningForAllAxis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pan",
                table: "Cads");

            migrationBuilder.AlterColumn<int>(
                name: "Z",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Camera's Z coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldComment: "Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "Y",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Camera's Y coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldComment: "Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "X",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Camera's X coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldComment: "X coordinate of 3D Model");

            migrationBuilder.AddColumn<int>(
                name: "PanX",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Panning along the x-axis of 3D Model");

            migrationBuilder.AddColumn<int>(
                name: "PanY",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Panning along the y-axis of 3D Model");

            migrationBuilder.AddColumn<int>(
                name: "PanZ",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Panning along the z-axis of 3D Model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PanX",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "PanY",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "PanZ",
                table: "Cads");

            migrationBuilder.AlterColumn<int>(
                name: "Z",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                comment: "Z coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Camera's Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "Y",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                comment: "Y coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Camera's Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "X",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                comment: "X coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Camera's X coordinate of 3D Model");

            migrationBuilder.AddColumn<int>(
                name: "Pan",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                defaultValue: 0,
                comment: "Panning along the y-axis of 3D Model");
        }
    }
}
