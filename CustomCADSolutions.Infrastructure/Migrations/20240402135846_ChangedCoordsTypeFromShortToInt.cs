using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class ChangedCoordsTypeFromShortToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Z",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                comment: "Z coordinate of 3D Model",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldMaxLength: 1000,
                oldComment: "Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "Y",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                comment: "Y coordinate of 3D Model",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldMaxLength: 1000,
                oldComment: "Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "X",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                comment: "X coordinate of 3D Model",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldMaxLength: 1000,
                oldComment: "X coordinate of 3D Model");

            migrationBuilder.UpdateData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "X", "Y", "Z" },
                values: new object[] { 750, 300, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Z",
                table: "Cads",
                type: "smallint",
                maxLength: 1000,
                nullable: false,
                comment: "Z coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldComment: "Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<short>(
                name: "Y",
                table: "Cads",
                type: "smallint",
                maxLength: 1000,
                nullable: false,
                comment: "Y coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldComment: "Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<short>(
                name: "X",
                table: "Cads",
                type: "smallint",
                maxLength: 1000,
                nullable: false,
                comment: "X coordinate of 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldComment: "X coordinate of 3D Model");

            migrationBuilder.UpdateData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "X", "Y", "Z" },
                values: new object[] { (short)750, (short)300, (short)0 });
        }
    }
}
