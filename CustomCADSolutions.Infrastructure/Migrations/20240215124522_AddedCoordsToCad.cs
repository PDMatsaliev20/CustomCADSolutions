using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    public partial class AddedCoordsToCad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadInBytes",
                table: "Cads");

            migrationBuilder.AddColumn<short>(
                name: "X",
                table: "Cads",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Y",
                table: "Cads",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Z",
                table: "Cads",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "Z",
                table: "Cads");

            migrationBuilder.AddColumn<byte[]>(
                name: "CadInBytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: true,
                comment: "Byte Array representing 3D Model");
        }
    }
}
