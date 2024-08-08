﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    public partial class AddedBytesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Bytes",
                table: "Cads",
                type: "varbinary(max)",
                nullable: true,
                comment: "Bytes of 3D Model");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "Cads");
        }
    }
}
