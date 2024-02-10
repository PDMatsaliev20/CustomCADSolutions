using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.AppWithIdentity.Data.Migrations
{
    public partial class ChangedCadUrlToFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Cads");

            migrationBuilder.AddColumn<byte[]>(
                name: "CadInBytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                comment: "Byte Array representing 3D Model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadInBytes",
                table: "Cads");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Url of 3D Model");
        }
    }
}
