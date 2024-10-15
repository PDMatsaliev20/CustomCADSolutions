using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Persistence.Migrations;

/// <inheritdoc />
public partial class ReplacedAttributesWithFluenAPI : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Manually added this method
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users");

        // Manually added this method
        migrationBuilder.DropUniqueConstraint(
            name: "AK_Roles_Name",
            table: "Roles");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "Users",
            type: "nvarchar(62)",
            maxLength: 62,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "RoleName",
            table: "Users",
            type: "nvarchar(20)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "nvarchar(62)",
            maxLength: 62,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            table: "Users",
            type: "nvarchar(62)",
            maxLength: 62,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Roles",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Roles",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Orders",
            type: "nvarchar(18)",
            maxLength: 18,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        // Manually added this method
        migrationBuilder.AddUniqueConstraint(
            name: "AK_Roles_Name",
            table: "Roles",
            column: "Name");

        // Manually added this method
        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users",
            column: "RoleName",
            principalTable: "Roles",
            principalColumn: "Name",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Manually added this method
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users");

        // Manually added this method
        migrationBuilder.DropUniqueConstraint(
            name: "AK_Roles_Name",
            table: "Roles");

        migrationBuilder.AlterColumn<string>(
            name: "UserName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(62)",
            oldMaxLength: 62);

        migrationBuilder.AlterColumn<string>(
            name: "RoleName",
            table: "Users",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(20)");

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(62)",
            oldMaxLength: 62,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(62)",
            oldMaxLength: 62,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Roles",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(20)",
            oldMaxLength: 20);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Roles",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(200)",
            oldMaxLength: 200);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Orders",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(18)",
            oldMaxLength: 18);

        // Manually added this method
        migrationBuilder.AddUniqueConstraint(
            name: "AK_Roles_Name",
            table: "Roles",
            column: "Name");

        // Manually added this method
        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleName",
            table: "Users",
            column: "RoleName",
            principalTable: "Roles",
            principalColumn: "Name",
            onDelete: ReferentialAction.Cascade);
    }
}
