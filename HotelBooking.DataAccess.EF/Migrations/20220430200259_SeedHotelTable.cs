using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBooking.DataAccess.EF.Migrations
{
    public partial class SeedHotelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hotel",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Cancun Hotel" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
