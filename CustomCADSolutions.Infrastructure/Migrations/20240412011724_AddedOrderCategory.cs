using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class AddedOrderCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Identification of Orders' 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identification of 3D model");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identification of Order's Category");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CategoryId",
                table: "Orders",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CategoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Identification of 3D model",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identification of Orders' 3D Model");
        }
    }
}
