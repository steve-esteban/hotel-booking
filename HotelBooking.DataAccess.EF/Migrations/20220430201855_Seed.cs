using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBooking.DataAccess.EF.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "Id", "HotelId", "Name" },
                values: new object[] { 1, 1, "Room 101" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
