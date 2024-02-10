using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class FixedOrderIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CadInBytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: true,
                comment: "Byte Array representing 3D Model",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldComment: "Byte Array representing 3D Model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "CadInBytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                comment: "Byte Array representing 3D Model",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true,
                oldComment: "Byte Array representing 3D Model");
        }
    }
}
