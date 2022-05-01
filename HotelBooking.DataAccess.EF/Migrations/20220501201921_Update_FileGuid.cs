using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBooking.DataAccess.EF.Migrations
{
    public partial class Update_FileGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ReservationGuid",
                table: "Reservation",
                column: "ReservationGuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservation_ReservationGuid",
                table: "Reservation");
        }
    }
}
