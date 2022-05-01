using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBooking.DataAccess.EF.Migrations
{
    public partial class Update_Unique_UserGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_UserGuid",
                table: "User",
                column: "UserGuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_UserGuid",
                table: "User");
        }
    }
}
