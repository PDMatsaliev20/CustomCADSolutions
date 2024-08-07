using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Path to Optional Image of Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Orders");
        }
    }
}
