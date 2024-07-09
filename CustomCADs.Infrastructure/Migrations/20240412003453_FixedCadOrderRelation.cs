using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    public partial class FixedCadOrderRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Identification of 3D model",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identification of 3D model");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                comment: "Identification of the creator of the 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComment: "Identification of the creator of the 3D Model");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Cads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "CreationDate of 3D Model",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldComment: "CreationDate of 3D Model");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Bytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                comment: "Bytes of 3D Model",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true,
                oldComment: "Bytes of 3D Model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Identification of 3D model",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identification of 3D model");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: true,
                comment: "Identification of the creator of the 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Identification of the creator of the 3D Model");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Cads",
                type: "datetime2",
                nullable: true,
                comment: "CreationDate of 3D Model",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "CreationDate of 3D Model");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Bytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: true,
                comment: "Bytes of 3D Model",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldComment: "Bytes of 3D Model");

            migrationBuilder.InsertData(
                table: "Cads",
                columns: new[] { "Id", "B", "Bytes", "CategoryId", "CreationDate", "CreatorId", "G", "IsValidated", "Name", "Price", "R", "SpinAxis", "X", "Y", "Z" },
                values: new object[] { 1, 255, null, 5, null, null, 255, false, "Chair", 0m, 255, "y", 750, 300, 0 });
        }
    }
}
