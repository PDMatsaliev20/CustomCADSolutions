using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class ForgotCadNavPropToOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CadId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Cads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CadId",
                table: "Orders",
                column: "CadId");

            migrationBuilder.CreateIndex(
                name: "IX_Cads_OrderId",
                table: "Cads",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cads_Orders_OrderId",
                table: "Cads",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cads_Orders_OrderId",
                table: "Cads");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CadId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Cads_OrderId",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Cads");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CadId",
                table: "Orders",
                column: "CadId",
                unique: true);
        }
    }
}
