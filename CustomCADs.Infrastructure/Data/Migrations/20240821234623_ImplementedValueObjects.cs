using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementedValueObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cads_Categories_CategoryId",
                table: "Cads");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CadPath",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "PanX",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "PanY",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "PanZ",
                table: "Cads");

            migrationBuilder.RenameColumn(
                name: "Z",
                table: "Cads",
                newName: "PanCoordZ");

            migrationBuilder.RenameColumn(
                name: "Y",
                table: "Cads",
                newName: "PanCoordY");

            migrationBuilder.RenameColumn(
                name: "X",
                table: "Cads",
                newName: "PanCoordX");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Status of Order");

            migrationBuilder.AlterColumn<bool>(
                name: "ShouldBeDelivered",
                table: "Orders",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Should Order Be Delivered After Completion");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Date of Order");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Name of Order");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComment: "Path to Optional Image of Order");

            migrationBuilder.AlterColumn<string>(
                name: "DesignerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true,
                oldComment: "Identification of Order's 3D Designer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Orders",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750,
                oldComment: "Description of Order");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identification of Order's Category");

            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Identification of Orders' 3D Model");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Identification of User");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identitfication of Order")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Cads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "3D Model Status");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Cads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Price of 3d model");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cads",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18,
                oldComment: "Name of 3D Model");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Relative Path to Image");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cads",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750,
                oldComment: "Description of 3D Model");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Identification of the creator of the 3D Model");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Cads",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "CreationDate of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Cads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Category of 3D Model");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identification of 3D Model")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<double>(
                name: "PanCoordZ",
                table: "Cads",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Camera's Z coordinate of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "PanCoordY",
                table: "Cads",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Camera's Y coordinate of 3D Model");

            migrationBuilder.AlterColumn<double>(
                name: "PanCoordX",
                table: "Cads",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldComment: "Camera's X coordinate of 3D Model");

            migrationBuilder.AddColumn<double>(
                name: "CamCoordX",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CamCoordY",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CamCoordZ",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Cads_Categories_CategoryId",
                table: "Cads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cads_Categories_CategoryId",
                table: "Cads");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CamCoordX",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "CamCoordY",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "CamCoordZ",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Cads");

            migrationBuilder.RenameColumn(
                name: "PanCoordZ",
                table: "Cads",
                newName: "Z");

            migrationBuilder.RenameColumn(
                name: "PanCoordY",
                table: "Cads",
                newName: "Y");

            migrationBuilder.RenameColumn(
                name: "PanCoordX",
                table: "Cads",
                newName: "X");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Status of Order",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "ShouldBeDelivered",
                table: "Orders",
                type: "bit",
                nullable: false,
                comment: "Should Order Be Delivered After Completion",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                comment: "Date of Order",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Name of Order",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Path to Optional Image of Order",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DesignerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                comment: "Identification of Order's 3D Designer",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Orders",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                comment: "Description of Order",
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Identification of Order's Category",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CadId",
                table: "Orders",
                type: "int",
                nullable: true,
                comment: "Identification of Orders' 3D Model",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Identification of User",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                comment: "Identitfication of Order",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "3D Model Status",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Cads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Price of 3d model",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cads",
                type: "nvarchar(18)",
                maxLength: 18,
                nullable: false,
                comment: "Name of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(18)",
                oldMaxLength: 18);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Relative Path to Image",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cads",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                comment: "Description of 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Identification of the creator of the 3D Model",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Cads",
                type: "datetime2",
                nullable: false,
                comment: "CreationDate of 3D Model",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Category of 3D Model",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cads",
                type: "int",
                nullable: false,
                comment: "Identification of 3D Model",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<double>(
                name: "Z",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Camera's Z coordinate of 3D Model",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "Y",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Camera's Y coordinate of 3D Model",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "X",
                table: "Cads",
                type: "float",
                nullable: false,
                comment: "Camera's X coordinate of 3D Model",
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "CadPath",
                table: "Cads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Relative Path to 3D Model");

            migrationBuilder.AddColumn<double>(
                name: "PanX",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Panning along the x-axis of 3D Model");

            migrationBuilder.AddColumn<double>(
                name: "PanY",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Panning along the y-axis of 3D Model");

            migrationBuilder.AddColumn<double>(
                name: "PanZ",
                table: "Cads",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Panning along the z-axis of 3D Model");

            migrationBuilder.AddForeignKey(
                name: "FK_Cads_Categories_CategoryId",
                table: "Cads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Categories_CategoryId",
                table: "Orders",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
