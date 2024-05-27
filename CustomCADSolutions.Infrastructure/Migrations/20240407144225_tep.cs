using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADSolutions.Infrastructure.Data.Migrations
{
    public partial class tep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "B", "G", "R" },
                values: new object[] { 255, 255, 255 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "B", "G", "R" },
                values: new object[] { 0, 0, 0 });
        }
    }
}
