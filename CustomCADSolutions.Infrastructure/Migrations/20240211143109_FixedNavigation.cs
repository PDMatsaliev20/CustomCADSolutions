using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class FixedNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cads_Orders_OrderId",
                table: "Cads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CadId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Cads_OrderId",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Cads");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                columns: new[] { "CadId", "BuyerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identification of Order")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Cads",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

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
    }
}
