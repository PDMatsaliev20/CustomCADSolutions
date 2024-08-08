using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MovedCadStorageToFrontend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "Cads");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Cads");

            migrationBuilder.AddColumn<byte[]>(
                name: "Bytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                comment: "Bytes of 3D Model");
        }
    }
}
