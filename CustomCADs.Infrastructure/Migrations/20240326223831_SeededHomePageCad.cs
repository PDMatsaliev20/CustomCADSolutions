using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomCADs.Infrastructure.Data.Migrations
{
    public partial class SeededHomePageCad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cads",
                columns: new[] { "Id", "Bytes", "CategoryId", "CreationDate", "CreatorId", "IsValidated", "Name", "SpinAxis", "SpinFactor", "X", "Y", "Z" },
                values: new object[] { 1, null, 5, null, null, false, "Chair", "y", -0.01, (short)750, (short)300, (short)0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cads",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
