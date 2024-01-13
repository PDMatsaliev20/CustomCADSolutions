using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Migrations
{
    public partial class FixedCadOrdering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CADs_CadId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "CadId",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "CADs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CADs_CadId",
                table: "Orders",
                column: "CadId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CADs_CadId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "CADs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
    }
}
