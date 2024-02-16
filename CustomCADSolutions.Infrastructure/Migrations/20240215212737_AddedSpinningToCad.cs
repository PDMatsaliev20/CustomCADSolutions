using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class AddedSpinningToCad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Z",
                table: "Cads",
                type: "smallint",
                nullable: false,
                comment: "Z coordinate of 3D Model",
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "Y",
                table: "Cads",
                type: "smallint",
                nullable: false,
                comment: "Y coordinate of 3D Model",
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "X",
                table: "Cads",
                type: "smallint",
                nullable: false,
                comment: "X coordinate of 3D Model",
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: true,
                comment: "Identification of the creator of the 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpinAxis",
                table: "Cads",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "",
                comment: "Spin axis of 3D Model");

            migrationBuilder.AddColumn<double>(
                name: "SpinFactor",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Spinning constant of 3D Model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpinAxis",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "SpinFactor",
                table: "Cads");

            migrationBuilder.AlterColumn<short>(
                name: "Z",
                table: "Cads",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<short>(
                name: "Y",
                table: "Cads",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<short>(
                name: "X",
                table: "Cads",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "X coordinate of 3D Model");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComment: "Identification of the creator of the 3D Model");
        }
    }
}
