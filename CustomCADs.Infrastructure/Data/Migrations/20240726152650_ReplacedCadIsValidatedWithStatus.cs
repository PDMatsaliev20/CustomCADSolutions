using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplacedCadIsValidatedWithStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "Cads");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "3D Model Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cads");

            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "Cads",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Is 3D Model validated");
        }
    }
}
