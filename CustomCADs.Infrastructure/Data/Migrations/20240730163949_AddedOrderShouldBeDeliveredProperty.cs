using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderShouldBeDeliveredProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldShow",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "ShouldBeDelivered",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Should Order Be Delivered After Completion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldBeDelivered",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "ShouldShow",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Should Order Be Visible After Completion");
        }
    }
}
