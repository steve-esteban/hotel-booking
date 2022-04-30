using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelBooking.DataAccess.EF.Migrations
{
    public partial class Update_ReservationDate_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Reservation",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_RoomId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Reservation");

            migrationBuilder.CreateTable(
                name: "ReservationDate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_ReservationDate",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Room_ReservationDate",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDate_ReservationId",
                table: "ReservationDate",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationDate_RoomId_Date",
                table: "ReservationDate",
                columns: new[] { "RoomId", "Date" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Reservation",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Reservation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Reservation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Reservation",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_RoomId",
                table: "Reservation",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Reservation",
                table: "Reservation",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
