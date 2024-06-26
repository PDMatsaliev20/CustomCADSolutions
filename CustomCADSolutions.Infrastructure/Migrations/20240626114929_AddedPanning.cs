using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPanning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pan",
                table: "Cads",
                type: "int",
                maxLength: 1000,
                nullable: false,
                defaultValue: 0,
                comment: "Panning along the y-axis of 3D Model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pan",
                table: "Cads");
        }
    }
}
