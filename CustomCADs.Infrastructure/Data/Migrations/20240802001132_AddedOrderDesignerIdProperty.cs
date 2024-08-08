using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderDesignerIdProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DesignerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                comment: "Identification of Order's 3D Designer");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DesignerId",
                table: "Orders",
                column: "DesignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_DesignerId",
                table: "Orders",
                column: "DesignerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_DesignerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DesignerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DesignerId",
                table: "Orders");
        }
    }
}
