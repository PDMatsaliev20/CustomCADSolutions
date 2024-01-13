using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Migrations
{
    public partial class UpdatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CADs_Categories_CategoryId",
                table: "CADs");

            migrationBuilder.DropForeignKey(
                name: "FK_CADs_Users_UserId",
                table: "CADs");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_CADs_CategoryId",
                table: "CADs");

            migrationBuilder.DropIndex(
                name: "IX_CADs_UserId",
                table: "CADs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CADs");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CADs",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "CADs",
                newName: "CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CADs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CADs_CategoryId",
                table: "CADs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CADs_UserId",
                table: "CADs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CADs_Categories_CategoryId",
                table: "CADs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CADs_Users_UserId",
                table: "CADs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
