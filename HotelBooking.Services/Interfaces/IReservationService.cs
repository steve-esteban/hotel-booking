using HotelBooking.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBooking.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<DateTime>> GetListOfBookedDatesAsync(int roomId);
        Task<IEnumerable<DateTime>> GetAvailableDatesAsync(int roomId);
        Task SaveReservationAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationByUserIdAsync(int userId);
        Task<Reservation> GetReservationAsync(int userId, Guid reservationGuid);
        Task RemoveReservationAsync(Reservation reservation);
        int GetReservationRoomId(Reservation reservation);
        Task UpdateReservationDateAsync(IEnumerable<ReservationDate> reservationDatesToRemove,
            IEnumerable<ReservationDate> reservationDatesToAdd);
        DateTime GetRoomDateTimeNow();
        DateTime GetDateFromNow(int days);
    }
}
