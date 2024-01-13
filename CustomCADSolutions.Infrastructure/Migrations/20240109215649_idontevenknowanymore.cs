using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Migrations
{
    public partial class idontevenknowanymore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CADs_CADId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CADId",
                table: "Orders",
                newName: "CadId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CADId",
                table: "Orders",
                newName: "IX_Orders_CadId");

            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CADs_CadId",
                table: "Orders",
                column: "CadId",
                principalTable: "CADs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CADs_CadId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CadId",
                table: "Orders",
                newName: "CADId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CadId",
                table: "Orders",
                newName: "IX_Orders_CADId");

            migrationBuilder.AlterColumn<int>(
                name: "CADId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                columns: new[] { "BuyerId", "CADId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CADs_CADId",
                table: "Orders",
                column: "CADId",
                principalTable: "CADs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
