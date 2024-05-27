using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    public partial class RemovedColorAndSpinAxisFromCad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "SpinAxis",
                table: "Cads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "SpinAxis",
                table: "Cads",
                type: "nvarchar(1)",
                nullable: true,
                comment: "Spin axis of 3D Model");
        }
    }
}
