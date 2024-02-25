using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class AddedCadValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cads",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                comment: "Name of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of 3D Model");

            migrationBuilder.AddColumn<bool>(
                name: "Validated",
                table: "Cads",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Is 3D Model validated");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validated",
                table: "Cads");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cads",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18,
                oldComment: "Name of 3D Model");
        }
    }
}
