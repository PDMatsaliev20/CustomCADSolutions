using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    public partial class FixedCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Cads",
                newName: "CategoryId");

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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Animals" },
                    { 2, "Characters" },
                    { 3, "Electronics" },
                    { 4, "Fashion" },
                    { 5, "Furniture" },
                    { 6, "Nature" },
                    { 7, "Science" },
                    { 8, "Sports" },
                    { 9, "Toys" },
                    { 10, "Vehicles" },
                    { 11, "Others" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cads_CategoryId",
                table: "Cads",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cads_Categories_CategoryId",
                table: "Cads",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cads_Categories_CategoryId",
                table: "Cads");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Cads_CategoryId",
                table: "Cads");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Cads",
                newName: "Category");
        }
    }
}
