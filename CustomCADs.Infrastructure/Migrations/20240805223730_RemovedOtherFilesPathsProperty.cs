using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOtherFilesPathsProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherFilesPaths",
                table: "Cads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherFilesPaths",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
