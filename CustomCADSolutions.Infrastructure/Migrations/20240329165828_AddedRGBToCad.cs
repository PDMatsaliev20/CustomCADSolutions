using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class AddedRGBToCad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpinFactor",
                table: "Cads");

            migrationBuilder.AddColumn<int>(
                name: "B",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "rgB value of 3D Model");

            migrationBuilder.AddColumn<int>(
                name: "G",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "rGb value of 3D Model");

            migrationBuilder.AddColumn<int>(
                name: "R",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Rgb value of 3D Model");

            migrationBuilder.UpdateData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "B",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "G",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "R",
                table: "Cads");

            migrationBuilder.AddColumn<double>(
                name: "SpinFactor",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Spinning constant of 3D Model");

            migrationBuilder.UpdateData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "SpinFactor" },
                values: new object[] { 0, -0.01 });
        }
    }
}
