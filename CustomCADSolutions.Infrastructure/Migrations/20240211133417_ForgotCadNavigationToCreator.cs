using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    public partial class ForgotCadNavigationToCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Cads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cads_CreatorId",
                table: "Cads",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cads_AspNetUsers_CreatorId",
                table: "Cads",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cads_AspNetUsers_CreatorId",
                table: "Cads");

            migrationBuilder.DropIndex(
                name: "IX_Cads_CreatorId",
                table: "Cads");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Cads");
        }
    }
}
