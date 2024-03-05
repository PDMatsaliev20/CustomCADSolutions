using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class FixedOrderOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cads_CadId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cads_CadId",
                table: "Orders",
                column: "CadId",
                principalTable: "Cads",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cads_CadId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cads_CadId",
                table: "Orders",
                column: "CadId",
                principalTable: "Cads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
