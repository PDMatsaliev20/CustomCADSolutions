using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCadCoordsFromIntToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Z",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Camera's Z coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Camera's Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "Y",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Camera's Y coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Camera's Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "X",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Camera's X coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Camera's X coordinate of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "PanZ",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Panning along the z-axis of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Panning along the z-axis of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "PanY",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Panning along the y-axis of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Panning along the y-axis of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "PanX",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Panning along the x-axis of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Panning along the x-axis of 3D Model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Z",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Camera's Z coordinate of 3D Model",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Camera's Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "Y",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Camera's Y coordinate of 3D Model",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Camera's Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "X",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Camera's X coordinate of 3D Model",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Camera's X coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "PanZ",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Panning along the z-axis of 3D Model",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Panning along the z-axis of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "PanY",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Panning along the y-axis of 3D Model",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Panning along the y-axis of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "PanX",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Panning along the x-axis of 3D Model",
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Panning along the x-axis of 3D Model");
        }
    }
}
