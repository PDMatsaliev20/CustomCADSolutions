using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class MadeSpinAxisNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpinAxis",
                table: "Cads",
                type: "nvarchar(1)",
                nullable: true,
                comment: "Spin axis of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldComment: "Spin axis of 3D Model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SpinAxis",
                table: "Cads",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "",
                comment: "Spin axis of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true,
                oldComment: "Spin axis of 3D Model");
        }
    }
}
